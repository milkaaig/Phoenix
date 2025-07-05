using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Phoenix.Data;
using Phoenix.Models;
using Microsoft.EntityFrameworkCore;
using Phoenix.Interfaces;


namespace Phoenix.Controllers

{
    public  class AddPost : Controller 
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AddPost> _logger;

        public AddPost(AppDbContext context, ILogger<AddPost> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Index page visited.");
            return View();
        }

        public IActionResult Posting()
        {
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
                // Return the form with validation errors
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

        [HttpGet]
        public IActionResult GetPosts()
        {

            _logger.LogInformation("GetPosts action called.");
            // Fetch all posts from the database
            _logger.LogInformation("Fetching all posts from the database.");
            if (_context.Posts == null)
            {
                _logger.LogWarning("No posts found in the database.");
                return View(new List<Post>());
            }
            var posts = _context.Posts.ToList();
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
    }
}

