using Dapper;
using System.Data;
using System.Data.SqlClient;
using MTADotNetCore.ConsoleApp;
using MTADotNetCore.ConsoleApp.EFCoreExamples;
// Console.WriteLine("Hello, World!");

// AdoDotNetExample adoDotNetExample = new AdoDotNetExample();
//adoDotNetExample.Read();
// adoDotNetExample.Create("title", "author", "content");
// adoDotNetExample.Update(6, "t1", "a1", "c1");
// adoDotNetExample.Delete(4);
// adoDotNetExample.Edit(50);
// adoDotNetExample.Edit(5);

// DapperExample dapperExample = new DapperExample();
// dapperExample.Run();

EFCoreExample eFCoreExample = new EFCoreExample();
eFCoreExample.Run();

Console.ReadLine();