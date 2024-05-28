using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace MTADotNetCore.Shared;

public class DapperService
{
    private readonly string _connectionString;

    public DapperService(string connectionString)
    { _connectionString = connectionString; }

    public List<M> Query<M>(string query, object? param = null)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        var lst = db.Query<M>(query, param).ToList();
        return lst;
    }

    public M QueryFirsrOrDefault<M>(string query, object? param = null)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        var item = db.Query<M>(query, param).FirstOrDefault();
        return item;
    }

    public int Execute(string query, object? param = null)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        var result = db.Execute(query, param);
        return result;
    }
}