namespace Assignment.Helpers;

using System.Data;
using Npgsql;

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DataTable ExecuteQuery(string query, NpgsqlParameter[] parameters = null)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(query, connection);
        if (parameters != null)
            command.Parameters.AddRange(parameters);

        using var adapter = new NpgsqlDataAdapter(command);
        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }

    public int ExecuteNonQuery(string query, NpgsqlParameter[] parameters = null)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(query, connection);
        if (parameters != null)
            command.Parameters.AddRange(parameters);

        return command.ExecuteNonQuery();
    }
}