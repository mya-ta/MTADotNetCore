using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTADotNetCore.RestApi;
using MTADotNetCore.RestApi.Model;
using MTADotNetCore.Shared;

namespace MTADotNetCore.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogDapper_2Controller : ControllerBase
{
    private readonly DapperService _dapperService =
        new(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);

    // Read
    [HttpGet]
    public IActionResult GetBlogs()
    {
        string query = "select * from tbl_blog";
        var lst = _dapperService.Query<BlogModel>(query);

        return Ok(lst);
    }

    [HttpGet("{id}")]
    public IActionResult GetBlog(int id)
    {
        //string query = "select * from tbl_blog where blogid = @BlogId";
        //using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        //var item = db.Query<BlogModel>(query, new BlogModel { BlogId = id }).FirstOrDefault();

        var item = FindById(id);
        if (item is null)
        {
            return NotFound("No data found for ID=" + id);
        }

        return Ok(item);
    }

    [HttpPost]
    public IActionResult CreateBlog(BlogModel blog)
    {
        string query = @"INSERT INTO [dbo].[Tbl_Blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent])
     VALUES
           (@BlogTitle
           ,@BlogAuthor       
           ,@BlogContent)";

        int result = _dapperService.Execute(query, blog);

        string message = result > 0 ? "Saving Successful." : "Saving Failed.";
        return Ok(message);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBlog(int id, BlogModel blog)
    {
        var item = FindById(id);
        if (item is null)
        {
            return NotFound("No data found for ID=" + id);
        }

        blog.BlogId = id;
        string query = @"UPDATE [dbo].[Tbl_Blog]
   SET [BlogTitle] = @BlogTitle
      ,[BlogAuthor] = @BlogAuthor
      ,[BlogContent] = @BlogContent
 WHERE BlogId = @BlogId";

        int result = _dapperService.Execute(query, blog);

        string message = result > 0 ? "Updating Successful for ID=" + id : "Updating Failed for ID=" + id;
        return Ok(message);
    }

    [HttpPatch("{id}")]
    public IActionResult PatchBlog(int id, BlogModel blog)
    {
        var item = FindById(id);
        if (item is null)
        {
            return NotFound("No data found for ID=" + id);
        }

        string conditions = string.Empty;
        if (!string.IsNullOrEmpty(blog.BlogTitle))
        {
            conditions += " [BlogTitle] = @BlogTitle, ";
        }

        if (!string.IsNullOrEmpty(blog.BlogAuthor))
        {
            conditions += " [BlogAuthor] = @BlogAuthor, ";
        }

        if (!string.IsNullOrEmpty(blog.BlogContent))
        {
            conditions += " [BlogContent] = @BlogContent, ";
        }

        if (conditions.Length == 0)
        {
            return NotFound("No data to update.");
        }

        conditions = conditions.Substring(0, conditions.Length - 2);
        blog.BlogId = id;

        string query = $@"UPDATE [dbo].[Tbl_Blog]
   SET {conditions}
 WHERE BlogId = @BlogId";

        int result = _dapperService.Execute(query, blog);

        string message = result > 0 ? "Updating Successful for ID=" + id : "Updating Failed for ID=" + id;
        return Ok(message);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBlog(int id)
    {
        var item = FindById(id);
        if (item is null)
        {
            return NotFound("No data found for ID=" + id);
        }

        string query = @"Delete From [dbo].[Tbl_Blog] WHERE BlogId = @BlogId";
        // int result = _dapperService.Execute(query, new BlogModel { BlogId = id });

        int result = _dapperService.Execute(query, new BlogModel { BlogId = id });


        string message = result > 0 ? "Deleting Successful for ID=" + id : "Deleting Failed for ID=" + id;
        return Ok(message);
    }

    private BlogModel? FindById(int id)
    {
        string query = "select * from tbl_blog where blogid = @BlogId";
        var item = _dapperService.QueryFirsrOrDefault<BlogModel>(query, new BlogModel { BlogId = id });

        return item;
    }
}