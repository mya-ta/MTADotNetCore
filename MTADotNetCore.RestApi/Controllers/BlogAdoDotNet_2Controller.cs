using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTADotNetCore.RestApi;
using MTADotNetCore.RestApi.Model;
using MTADotNetCore.Shared;

namespace MTADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdoDotNet_2Controller : ControllerBase
    {
        private readonly AdoDotNetService _adoDotNetService = new(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);

        [HttpGet]
        public IActionResult GetBlogs()
        {
            string query = "select * from tbl_blog";
            var lst = _adoDotNetService.Query<BlogModel>(query);

            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            string query = "select * from tbl_blog where BlogId = @BlogId";

            // AdoDotNetParameter[] parameters = [new AdoDotNetParameter("@BlogId", id)];
            // var lst = _adoDotNetService.Query<BlogModel>(query, parameters);

            // AdoDotNetParameter[] parameters = new AdoDotNetParameter[1];
            // parameters[0] = new AdoDotNetParameter("@BlogId", id);
            // var lst = _adoDotNetService.Query<BlogModel>(query, parameters);

            var item = _adoDotNetService.QueryFirstOrDefault<BlogModel>(query,
            new AdoDotNetParameter("@BlogId", id));

            if (item is null) return NotFound("No data found for ID=" + id);

            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateBlog(BlogModel blog)
        {
            string query = @"INSERT INTO [dbo].[tbl_blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent])
     VALUES
           (@BlogTitle
           ,@BlogAuthor
           ,@BlogContent)";

            int result = _adoDotNetService.Execute(query,
            new AdoDotNetParameter("@BlogTitle", blog.BlogTitle),
            new AdoDotNetParameter("@BlogAuthor", blog.BlogAuthor),
            new AdoDotNetParameter("@BlogContent", blog.BlogContent));

            string message = result > 0 ? "Saving Success." : "Saving Failed.";

            return Ok(message);
            // return StatusCode(500, message);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlogs(int id, BlogModel blog)
        {
            // var item = FindById(id);
            // if (item is null)
            // {
            //     return NotFound("No data found for ID=" + id);
            // }

            string query = @"UPDATE [dbo].[tbl_blog]
        SET [BlogTitle] = @BlogTitle
            ,[BlogAuthor] = @BlogAuthor
            ,[BlogContent] = @BlogContent
        WHERE BlogId = @BlogId";

            int result = _adoDotNetService.Execute(query,
            new AdoDotNetParameter("@BlogId", id),
            new AdoDotNetParameter("@BlogTitle", blog.BlogTitle),
            new AdoDotNetParameter("@BlogAuthor", blog.BlogAuthor),
            new AdoDotNetParameter("@BlogContent", blog.BlogContent));

            string message = result > 0 ? "Updating Successful for ID=" + id : "Updating Failed for ID=" + id;

            return Ok(message);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogModel blog)
        {
            List<AdoDotNetParameter> lst = [];

            string conditions = string.Empty;
            if (!string.IsNullOrEmpty(blog.BlogTitle))
            {
                conditions += "[BlogTitle]=@BlogTitle,";
                lst.Add("@BlogTitle", blog.BlogTitle);
            }

            if (!string.IsNullOrEmpty(blog.BlogAuthor))
            {
                conditions += "[BlogAuthor]=@BlogAuthor,";
                lst.Add("@BlogAuthor", blog.BlogAuthor);
            }

            if (!string.IsNullOrEmpty(blog.BlogContent))
            {
                conditions += "[BlogContent]=@BlogContent,";
                lst.Add("@BlogContent", blog.BlogContent);
            }

            conditions = conditions.Substring(0, conditions.Length - 1);

            if (conditions.Length == 0)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                return NotFound(response);
            }

            lst.Add(new AdoDotNetParameter("@BlogId", id));

            string query = $@"UPDATE [dbo].[Tbl_Blog]
                            SET {conditions} WHERE BlogId=@BlogId";

            int result = _adoDotNetService.Execute(query, lst.ToArray());

            string message = result > 0 ? "Updating Successful for ID=" + id : "Updating Failed for ID=" + id;
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            // var item = FindById(id);
            // if (item is null)
            // { return NotFound("No data found for ID=" + id); }

            string query = @"DELETE FROM [dbo].[tbl_blog] WHERE BlogId = @BlogId";

            int result = _adoDotNetService.Execute(query, new AdoDotNetParameter("@BlogId", id));

            string message = result > 0 ? "Deleting Successful for ID=" + id : "Deleting Failed for ID=" + id;
            return Ok(message);
        }

        private BlogModel FindById(int id)
        {
            string query = "select * from tbl_blog where blogId = @BlogId";
            var item = _adoDotNetService.QueryFirstOrDefault<BlogModel>(query, new AdoDotNetParameter("@BlogId", id));

            return item;
        }
    }
}