using Microsoft.AspNetCore.Mvc;
using Phoenix.Data;
using Phoenix.Models;
using Microsoft.EntityFrameworkCore;



namespace Phoenix.Controllers
{
    public class AddPost : Controller
    {
        private readonly AppDbContext _context;
        public AddPost(AppDbContext context)
        {
            _context = context;
            
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

           



            _context.Add(post);
            _context.SaveChanges();

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
        public  IActionResult GetPosts()
        {
            var posts =  _context.Posts.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
                 
                var post = _context.Posts.Find(id);
                if (post == null)
                {
                    return NotFound();
                }
                _context.Posts.Remove(post);
                _context.SaveChanges();
                return View();
            
        }
    }
}
