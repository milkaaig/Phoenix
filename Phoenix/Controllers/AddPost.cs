using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Phoenix.Data;
using Phoenix.Models;
using Microsoft.EntityFrameworkCore;
using Phoenix.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text;


namespace Phoenix.Controllers

{

    public class AddPost : Controller 
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AddPost> _logger;

        
        public AddPost(AppDbContext context, ILogger<AddPost> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogWarning("Index page");
            return View();
        }

        public IActionResult Posting()
        {
            _logger.LogWarning("post submitted.");
            return View();
        }


        public IActionResult AddPosts()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitPost(Post post)

        {


            if (!ModelState.IsValid)
            {
                _logger.LogError("Model state is invalid in SubmitPost action.");

                return View("AddPosts", post);


            }

            _context.Add(post);
            _context.SaveChanges();

            // posting on author database
            var author = new Author
            {
                Name = post.Author,
                Post = post
            };
            _context.Authors.Add(author);
            _context.SaveChanges();

           

            return View("Posting");
        }

        public IActionResult GetPosts(string search, string category, string sort, int page = 1)
        {
            const int pageSize = 15;
            var postsQuery = _context.Posts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                _logger.LogWarning("search.");

                string lowered = search.ToLower();
                postsQuery = postsQuery.Where(p =>
                    p.Title.ToLower().Contains(lowered) ||
                    p.Category.ToLower().Contains(lowered) ||
                    p.Author.ToLower().Contains(lowered)
                );
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                postsQuery = postsQuery.Where(p => p.Category == category);
            }

            // Sorting
            if (sort == "latest")
            {
                postsQuery = postsQuery.OrderByDescending(p => p.date);
            }
            else if (sort == "oldest")
            {
                postsQuery = postsQuery.OrderBy(p => p.date);
            }
            else
            {
                postsQuery = postsQuery.OrderByDescending(p => p.date); // Default: latest
            }

            ViewBag.Categories = _context.Posts.Select(p => p.Category).Distinct().ToList() ?? new List<string>();

            // Pagination logic
            int totalPosts = postsQuery.Count();
            int totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);

            // Only limit to 15 posts if no filter/sort/search is applied
            bool isInitialLoad = string.IsNullOrWhiteSpace(search) && string.IsNullOrWhiteSpace(category) && string.IsNullOrWhiteSpace(sort);
            var posts = isInitialLoad
                ? postsQuery.Take(pageSize).ToList()
                : postsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.IsInitialLoad = isInitialLoad;

            return View(posts);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
            }
            return RedirectToAction("AllPosts");
        }

        [HttpPost]
        public IActionResult DeleteAll()
        {
            _context.Posts.RemoveRange(_context.Posts);
            _context.SaveChanges();
            return RedirectToAction("AllPosts");
        }

        public IActionResult AllPosts()

        {

            var posts = _context.Posts.ToList();
            ViewBag.HasPosts = posts.Any();
            return View(posts);
        }

        public IActionResult Post(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null) return NotFound();
            return View(post);
        }

        // GET: to view what  is  to be edited
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null) return NotFound();
            return View(post);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Post post)
        {
            if (id != post.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(post);
                _context.SaveChanges();
                return RedirectToAction("Post", new { id = post.Id });
            }
            return View(post);
        }

        [HttpGet]
        public IActionResult ExportPosts()
        {

            _logger.LogWarning("export has been clicked");
            var posts = _context.Posts.ToList();
            var filePath = Path.Combine("Exports", $"posts-{DateTime.Now:yyyyMMddHHmmss}.csv");

            Directory.CreateDirectory("Exports");
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine("Id,Title,Description,Date,Category,Author");
                foreach (var post in posts)
                {
                    writer.WriteLine($"{post.Id},\"{post.Title}\",\"{post.Description}\",{post.date:yyyy-MM-dd},{post.Category},{post.Author}");
                }
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "text/csv", Path.GetFileName(filePath));
        }
    }

    public class AddPostsModel : PageModel
    {
        private readonly ILogger<AddPostsModel> _logger;

        public AddPostsModel(ILogger<AddPostsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogWarning("This is a test warning from Serilog");
            }
    }
}

