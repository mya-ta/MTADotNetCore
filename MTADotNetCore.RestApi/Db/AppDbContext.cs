using System.Data;
using System.Data.SqlClient;
using System.Net;
using Microsoft.EntityFrameworkCore;
using MTADotNetCore.RestApi.Models;

namespace MTADotNetCore.RestApi.Db;

internal class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
    }
    public DbSet<BlogModel> Blogs { get; set; }
}