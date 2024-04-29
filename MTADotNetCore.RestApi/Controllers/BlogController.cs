using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTADotNetCore.RestApi.Db;
using MTADotNetCore.RestApi.Models;

namespace MTADotNetCore.RestApi.Controllers
{
    //endpoint => api/blog
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BlogController()
        {
            _context = new AppDbContext();
        }

        [HttpGet]
        public IActionResult Read()
        {
            var lst = _context.Blogs.ToList();
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (item is null)
            {
                return NotFound("No data found for ID = " + id);
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create(BlogModel blog)
        {
            _context.Blogs.Add(blog);
            var result = _context.SaveChanges();

            string message = result > 0 ? "Saving Success." : "Saving Failed.";
            return Ok(message);
        }

        [HttpPut("{id}")] //update whole object
        public IActionResult Update(int id, BlogModel blog)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (item is null)
            {
                return NotFound("No data found for ID = " + id);
            }
            item.BlogTitle = blog.BlogTitle;
            item.BlogAuthor = blog.BlogAuthor;
            item.BlogContent = blog.BlogContent;

            int result = _context.SaveChanges();
            string message = result > 0 ? "Updating Success for ID = " + id : "Updating Failed for ID = " + id;

            return Ok(message);
        }

        [HttpPatch("{id}")] //update 1 by 1
        public IActionResult Patch(int id, BlogModel blog)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (item is null)
            {
                return NotFound("No data found for ID = " + id);
            }

            if (!string.IsNullOrEmpty(blog.BlogTitle))
            {
                item.BlogTitle = blog.BlogTitle;
            }
            if (!string.IsNullOrEmpty(blog.BlogAuthor))
            {
                item.BlogAuthor = blog.BlogAuthor;
            }
            if (!string.IsNullOrEmpty(blog.BlogContent))
            {
                item.BlogContent = blog.BlogContent;
            }

            int result = _context.SaveChanges();
            string message = result > 0 ? "Updating Success for ID = " + id : "Updating Failed for ID = " + id;
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (item is null)
            {
                return NotFound("No data found for ID = " + id);
            }

            _context.Blogs.Remove(item);
            int result = _context.SaveChanges();

            string message = result > 0 ? "Deleting Success for ID = " + id : "Deleting Failed for ID = " + id;
            return Ok(message);
        }
    }
}