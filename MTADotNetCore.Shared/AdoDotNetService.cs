using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace MTADotNetCore.Shared;

public class AdoDotNetService
{
    private readonly string _connectionString;

    public AdoDotNetService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<T> Query<T>(string query, params AdoDotNetParameter[]? parameters)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand(query, connection);
        if (parameters is not null && parameters.Length > 0)
        {
            // foreach (var item in parameters)
            // { cmd.Parameters.AddWithValue(item.Key, item.Value); }

            // var parameterArray = parameters.Select(item => new SqlParameter(item.Key, item.Value)).ToArray();
            // cmd.Parameters.AddRange(parameterArray);

            cmd.Parameters.AddRange(parameters.Select(item =>
            new SqlParameter(item.Key, item.Value)).ToArray());
        }

        SqlDataAdapter SqlDataAdapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        SqlDataAdapter.Fill(dt);

        connection.Close();

        string json = JsonConvert.SerializeObject(dt); // C# object to json
        List<T> lst = JsonConvert.DeserializeObject<List<T>>(json); // json to c# object

        return lst;
    }

    public T QueryFirstOrDefault<T>(string query, params AdoDotNetParameter[]? parameters)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand(query, connection);
        if (parameters is not null && parameters.Length > 0)
        {
            cmd.Parameters.AddRange(parameters.Select(item =>
            new SqlParameter(item.Key, item.Value)).ToArray());
        }

        SqlDataAdapter SqlDataAdapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        SqlDataAdapter.Fill(dt);

        connection.Close();

        string json = JsonConvert.SerializeObject(dt); // C# object to json
        List<T> lst = JsonConvert.DeserializeObject<List<T>>(json); // json to c# object

        return lst[0];
    }

    public int Execute(string query, params AdoDotNetParameter[]? parameters)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        SqlCommand cmd = new SqlCommand(query, connection);
        if (parameters is not null && parameters.Length > 0)
        {
            cmd.Parameters.AddRange(parameters.Select(item =>
            new SqlParameter(item.Key, item.Value)).ToArray());
        }
        var result = cmd.ExecuteNonQuery();

        connection.Close();
        return result;
    }
}

public class AdoDotNetParameter
{
    public AdoDotNetParameter() { }
    public AdoDotNetParameter(string key, object value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; set; }
    public object Value { get; set; }
}

public static class AdoDotNetParameterListExtension
{
    public static List<AdoDotNetParameter> Add(this List<AdoDotNetParameter> lst, string key, object value)
    {
        lst.Add(new AdoDotNetParameter(key, value));
        return lst;
    }
}