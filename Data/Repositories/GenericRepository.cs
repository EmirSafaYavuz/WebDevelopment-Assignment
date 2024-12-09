namespace Assignment.Data.Repositories;

using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly string _connectionString;

    public GenericRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var query = $"SELECT * FROM public.\"{typeof(T).Name}\"";
        var result = new List<T>();

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new NpgsqlCommand(query, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    // Mapping yapılacak (Reflection ile veya manuel)
                    var entity = Activator.CreateInstance<T>();
                    foreach (var property in typeof(T).GetProperties())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                        {
                            property.SetValue(entity, reader[property.Name]);
                        }
                    }
                    result.Add(entity);
                }
            }
        }
        return result;
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var query = $"SELECT * FROM public.\"{typeof(T).Name}\" WHERE \"{typeof(T).Name}ID\" = @Id";
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var entity = Activator.CreateInstance<T>();
                        foreach (var property in typeof(T).GetProperties())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                            {
                                property.SetValue(entity, reader[property.Name]);
                            }
                        }
                        return entity;
                    }
                }
            }
        }
        return null;
    }

    public async Task AddAsync(T entity)
    {
        // Insert SQL sorgusunu oluştur
        var query = GenerateInsertQuery(entity);

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(query, connection))
            {
                AddParameters(command, entity);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateAsync(T entity)
    {
        // Update SQL sorgusunu oluştur
        var query = GenerateUpdateQuery(entity);

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(query, connection))
            {
                AddParameters(command, entity);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task DeleteAsync(int id)
    {
        var query = $"DELETE FROM public.\"{typeof(T).Name}\" WHERE \"{typeof(T).Name}ID\" = @Id";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    // Yardımcı metotlar
    private string GenerateInsertQuery(T entity)
    {
        var properties = typeof(T).GetProperties();
        var columns = string.Join(", ", properties.Select(p => $"\"{p.Name}\""));
        var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
        return $"INSERT INTO public.\"{typeof(T).Name}\" ({columns}) VALUES ({values})";
    }

    private string GenerateUpdateQuery(T entity)
    {
        var properties = typeof(T).GetProperties().Where(p => p.Name != $"{typeof(T).Name}ID");
        var setClause = string.Join(", ", properties.Select(p => $"\"{p.Name}\" = @{p.Name}"));
        return $"UPDATE public.\"{typeof(T).Name}\" SET {setClause} WHERE \"{typeof(T).Name}ID\" = @{typeof(T).Name}ID";
    }

    private void AddParameters(NpgsqlCommand command, T entity)
    {
        foreach (var property in typeof(T).GetProperties())
        {
            command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(entity) ?? DBNull.Value);
        }
    }
}