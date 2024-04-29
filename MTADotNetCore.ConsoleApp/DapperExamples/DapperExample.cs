using Dapper;
using MTADotNetCore.ConsoleApp.Dtos;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace MTADotNetCore.ConsoleApp.DapperExamples;

public class DapperExample
{
    public void Run()
    {
        Read();
        // Edit(1);
        // Edit(20);
        // Create("title tt", "author tt", "content tt");
        // Update(100, "title 2", "author 2", "content 2");
        // Delete(100);
        // Delete(1002);
    }
    private void Read()
    {
        using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        List<BlogDto> lst = db.Query<BlogDto>("select * from tbl_blog").ToList();

        foreach (BlogDto item in lst)
        {
            Console.WriteLine(item.BlogId);
            Console.WriteLine(item.BlogTitle);
            Console.WriteLine(item.BlogAuthor);
            Console.WriteLine(item.BlogContent);
            Console.WriteLine("-------------------------");
        }
    }
    private void Edit(int id)
    {
        using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        var item = db.Query<BlogDto>("select * from tbl_blog where blogId = @BlogId", new BlogDto { BlogId = id }).FirstOrDefault();
        if (item is null)
        {
            Console.WriteLine("No data found for ID = " + id);
            return;
        }
        Console.WriteLine(item.BlogId);
        Console.WriteLine(item.BlogTitle);
        Console.WriteLine(item.BlogAuthor);
        Console.WriteLine(item.BlogContent);

    }
    private void Create(string title, string author, string content)
    {
        var item = new BlogDto
        {
            BlogTitle = title,
            BlogAuthor = author,
            BlogContent = content
        };
        string query = @"INSERT INTO [dbo].[tbl_blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent])
     VALUES
           (@BlogTitle
           ,@BlogAuthor
           ,@BlogContent)";

        using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        int result = db.Execute(query, item);

        string message = result > 0 ? "Saving Success." : "Saving Failed.";
        Console.WriteLine(message);

    }
    private void Update(int id, string title, string author, string content)
    {
        var item = new BlogDto
        {
            BlogId = id,
            BlogTitle = title,
            BlogAuthor = author,
            BlogContent = content
        };
        string query = @"UPDATE [dbo].[tbl_blog]
        SET [BlogTitle] = @BlogTitle
            ,[BlogAuthor] = @BlogAuthor
            ,[BlogContent] = @BlogContent
        WHERE BlogId = @BlogId";

        using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        int result = db.Execute(query, item);

        string message = result > 0 ? "Updating Success for ID = " + id : "Updating Failed for ID = " + id;
        Console.WriteLine(message);
    }
    private void Delete(int id)
    {
        var item = new BlogDto
        {
            BlogId = id,
        };
        string query = @"DELETE FROM [dbo].[tbl_blog] WHERE BlogId = @BlogId";

        using IDbConnection db = new SqlConnection(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
        int result = db.Execute(query, item);

        string message = result > 0 ? "Deleting Success for ID = " + id : "Deleting Failed for ID = " + id;
        Console.WriteLine(message);
    }

}