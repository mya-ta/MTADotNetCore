using System.Data.SqlClient;

namespace MTADotNetCore.ConsoleApp.Services;

internal static class ConnectionStrings
{
    public static SqlConnectionStringBuilder SqlConnectionStringBuilder = new SqlConnectionStringBuilder()
    {
        DataSource = ".",
        InitialCatalog = "DotNetTrainingB4",
        UserID = "sa",
        Password = "sa@123",
        TrustServerCertificate = true
    };
}