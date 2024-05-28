using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTADotNetCore.RestApi;
using MTADotNetCore.RestApi.Model;

namespace MTADotNetCore.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogDapperController : ControllerBase
{
    [HttpGet]
    public IActionResult GetBlogs()
    {
        string query = "select * from tbl_blog";
        using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        List<BlogModel> lst = db.Query<BlogModel>(query).ToList();
        return Ok(lst);
    }

    [HttpGet("{id}")]
    public IActionResult GetBlog(int id)
    {
        // string query = "select * from tbl_blog where blogId = @BlogId";
        // using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        // var item = db.Query<BlogModel>(query, new BlogModel { BlogId = id }).FirstOrDefault();
        var item = FindById(id);

        if (item is null)
        {
            return NotFound("No data found for ID=" + id);
        }
        return Ok(item);
    }

    [HttpPost]
    public IActionResult CreateBlogs(BlogModel blog)
    {
        string query = @"INSERT INTO [dbo].[tbl_blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent])
     VALUES
           (@BlogTitle
           ,@BlogAuthor
           ,@BlogContent)";

        using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        int result = db.Execute(query, blog);

        string message = result > 0 ? "Saving Successful." : "Saving Failed.";
        return Ok(message);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBlogs(int id, BlogModel blog)
    {
        var item = FindById(id);
        if (item is null)
        {
            return NotFound("No data found for ID=" + id);
        }

        blog.BlogId = id;
        string query = @"UPDATE [dbo].[tbl_blog]
        SET [BlogTitle] = @BlogTitle
            ,[BlogAuthor] = @BlogAuthor
            ,[BlogContent] = @BlogContent
        WHERE BlogId = @BlogId";

        using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        int result = db.Execute(query, blog);

        string message = result > 0 ? "Updating Successful for ID=" + id : "Updating Failed for ID=" + id;
        return Ok(message);
    }

    [HttpPatch("{id}")]
    public IActionResult PatchBlogs(int id, BlogModel blog)
    {
        var item = FindById(id);
        if (item is null)
        {
            return NotFound("No data found for ID=" + id);
        }

        string conditions = string.Empty;
        if (!string.IsNullOrEmpty(blog.BlogTitle))
        { conditions += "[BlogTitle] = @BlogTitle],"; }

        if (!string.IsNullOrEmpty(blog.BlogAuthor))
        { conditions += "[BlogAuthor] = @BlogAuthor,"; }

        if (!string.IsNullOrEmpty(blog.BlogContent))
        { conditions += "[BlogContent] = @BlogContent,"; }

        if (conditions.Length == 0)
        { return NotFound("No data to update for ID=" + id); }

        conditions = conditions.Substring(0, conditions.Length - 1);
        blog.BlogId = id;

        string query = $@"UPDATE [dbo].[tbl_blog]
        SET {conditions} WHERE BlogId = @BlogId";

        using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        int result = db.Execute(query, blog);

        string message = result > 0 ? "Updating Successful for ID=" + id : "Updating Failed for ID=" + id;
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBlogs(int id)
    {
        var item = FindById(id);
        if (item is null)
        {
            return NotFound("No data found for ID=" + id);
        }

        string query = @"DELETE FROM [dbo].[tbl_blog] WHERE BlogId = @BlogId";
        using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        int result = db.Execute(query, new BlogModel { BlogId = id });

        string message = result > 0 ? "Deleting Successful for ID=" + id : "Deleting Failed for ID=" + id;
        return Ok(message);
    }

    private BlogModel FindById(int id)
    {
        string query = "select * from tbl_blog where blogId = @BlogId";
        using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        var item = db.Query<BlogModel>(query, new BlogModel { BlogId = id }).FirstOrDefault();
        return item;
    }
}