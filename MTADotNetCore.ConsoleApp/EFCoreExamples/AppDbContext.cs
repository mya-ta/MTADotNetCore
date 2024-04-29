using System.Data;
using System.Data.SqlClient;
using System.Net;
using Microsoft.EntityFrameworkCore;
using MTADotNetCore.ConsoleApp.Dtos;
using MTADotNetCore.ConsoleApp.Services;

namespace MTADotNetCore.ConsoleApp.EFCoreExamples;

internal class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionStrings.SqlConnectionStringBuilder.ConnectionString);
    }
    public DbSet<BlogDto> Blogs { get; set; }
}