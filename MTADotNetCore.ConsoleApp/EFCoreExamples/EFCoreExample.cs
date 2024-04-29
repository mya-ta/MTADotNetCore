using System.Data;
using System.Data.SqlClient;
using System.Net;
using MTADotNetCore.ConsoleApp.Dtos;

namespace MTADotNetCore.ConsoleApp.EFCoreExamples;

internal class EFCoreExample
{
    private readonly AppDbContext db = new AppDbContext();

    public void Run()
    {
        // Read();
        // Edit(80);
        // Edit(8);
        // Create("title tt", "author tt", "content tt");
        // Update(100, "title 2", "author 2", "content 2");
        Update(1003, "title 2", "author 2", "content 2");
        Delete(100);
        Delete(1003);
    }
    private void Read()
    {
        var lst = db.Blogs.ToList();
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
        var item = db.Blogs.FirstOrDefault(x => x.BlogId == id);
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
        db.Blogs.Add(item);
        int result = db.SaveChanges();
        string message = result > 0 ? "Saving Success." : "Saving Failed.";
        Console.WriteLine(message);

    }
    private void Update(int id, string title, string author, string content)
    {
        var item = db.Blogs.FirstOrDefault(x => x.BlogId == id);
        if (item is null)
        {
            Console.WriteLine("No data found for ID = " + id);
            return;
        }
        item.BlogTitle = title;
        item.BlogAuthor = author;
        item.BlogContent = content;

        int result = db.SaveChanges();
        string message = result > 0 ? "Updating Success for ID = " + id : "Updating Failed for ID = " + id;
        Console.WriteLine(message);
    }
    private void Delete(int id)
    {
        var item = db.Blogs.FirstOrDefault(x => x.BlogId == id);
        if (item is null)
        {
            Console.WriteLine("No data found for ID = " + id);
            return;
        }
        db.Blogs.Remove(item);
        int result = db.SaveChanges();

        string message = result > 0 ? "Deleting Success for ID = " + id : "Deleting Failed for ID = " + id;
        Console.WriteLine(message);
    }
}