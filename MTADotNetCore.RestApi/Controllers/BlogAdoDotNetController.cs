using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTADotNetCore.RestApi;
using MTADotNetCore.RestApi.Model;

namespace MTADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdoDotNetController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBlogs()
        {
            string query = "select * from tbl_blog";

            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter SqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            SqlDataAdapter.Fill(dt);

            connection.Close();

            // List<BlogModel> lst = new List<BlogModel>();
            // foreach (DataRow dr in dt.Rows)
            // {
            //     BlogModel blog = new BlogModel();
            //     blog.BlogId = Convert.ToInt32(dr["BlogId"]);
            //     blog.BlogTitle = Convert.ToString(dr["BlogTitle"]);
            //     blog.BlogAuthor = Convert.ToString(dr["BlogAuthor"]);
            //     blog.BlogContent = Convert.ToString(dr["BlogContent"]);
            //     lst.Add(blog);
            // }

            List<BlogModel> lst = dt.AsEnumerable().Select(dr => new BlogModel
            {
                BlogId = Convert.ToInt32(dr["BlogId"]),
                BlogTitle = Convert.ToString(dr["BlogTitle"]),
                BlogAuthor = Convert.ToString(dr["BlogAuthor"]),
                BlogContent = Convert.ToString(dr["BlogContent"])
            }).ToList();

            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            string query = "select * from tbl_blog where BlogId = @BlogId";

            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            SqlDataAdapter SqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            SqlDataAdapter.Fill(dt);

            connection.Close();

            if (dt.Rows.Count == 0) return NotFound("No data found for ID=" + id);

            DataRow dr = dt.Rows[0];
            var item = new BlogModel
            {
                BlogId = Convert.ToInt32(dr["BlogId"]),
                BlogTitle = Convert.ToString(dr["BlogTitle"]),
                BlogAuthor = Convert.ToString(dr["BlogAuthor"]),
                BlogContent = Convert.ToString(dr["BlogContent"])
            };

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

            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogTitle", blog.BlogTitle);
            cmd.Parameters.AddWithValue("@BlogAuthor", blog.BlogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", blog.BlogContent);
            int result = cmd.ExecuteNonQuery();

            connection.Close();
            string message = result > 0 ? "Saving Success." : "Saving Failed.";

            return Ok(message);
            // return StatusCode(500, message);
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

            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            int result = connection.Execute(query, blog);

            connection.Close();
            string message = result > 0 ? "Updating Successful for ID=" + id : "Updating Failed for ID=" + id;

            return Ok(message);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogModel blog)
        {
            string conditions = string.Empty;
            if (!string.IsNullOrEmpty(blog.BlogTitle))
            { conditions += "[BlogTitle]=@BlogTitle,"; }

            if (!string.IsNullOrEmpty(blog.BlogAuthor))
            { conditions += "[BlogAuthor]=@BlogAuthor,"; }

            if (!string.IsNullOrEmpty(blog.BlogContent))
            { conditions += "[BlogContent]=@BlogContent,"; }

            conditions = conditions.Substring(0, conditions.Length - 1);
            if (conditions.Length == 0)
            { return NotFound("No Data Found"); }

            string query = $@"UPDATE [dbo].[Tbl_Blog]
                            SET {conditions} WHERE BlogId=@BlogId";

            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            cmd.Parameters.AddWithValue("@BlogTitle", blog.BlogTitle);
            cmd.Parameters.AddWithValue("@BlogAuthor", blog.BlogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", blog.BlogContent);

            int result = cmd.ExecuteNonQuery();
            connection.Close();

            string message = result > 0 ? "Updating Successful for ID=" + id : "Updating Failed for ID=" + id;
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            var item = FindById(id);
            if (item is null)
            { return NotFound("No data found for ID=" + id); }

            string query = @"DELETE FROM [dbo].[tbl_blog] WHERE BlogId = @BlogId";

            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            int result = connection.Execute(query, new BlogModel { BlogId = id });

            connection.Close();
            string message = result > 0 ? "Deleting Successful for ID=" + id : "Deleting Failed for ID=" + id;
            return Ok(message);
        }

        private BlogModel FindById(int id)
        {
            string query = "select * from tbl_blog where blogId = @BlogId";

            SqlConnection connection = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            var item = connection.QueryFirstOrDefault<BlogModel>(query, new BlogModel { BlogId = id });
            connection.Close();

            return item;
        }
    }
}