using Assignment.Models;
using Npgsql;

namespace Assignment.Data.Repositories
{
    public class CategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Get All Categories
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = new List<Category>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT ""CategoryID"", ""CategoryName"", ""Description"", ""Active"" 
                              FROM ""Category"";";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            categories.Add(new Category
                            {
                                CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                Description = reader["Description"] as string,
                                Active = reader.GetBoolean(reader.GetOrdinal("Active"))
                            });
                        }
                    }
                }
            }

            return categories;
        }

        // Get Category by ID
        public async Task<Category> GetByIdAsync(int id)
        {
            Category category = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT ""CategoryID"", ""CategoryName"", ""Description"", ""Active"" 
                              FROM ""Category"" WHERE ""CategoryID"" = @CategoryID;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryID", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            category = new Category
                            {
                                CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                Description = reader["Description"] as string,
                                Active = reader.GetBoolean(reader.GetOrdinal("Active"))
                            };
                        }
                    }
                }
            }

            return category;
        }

        // Add a New Category
        public async Task AddAsync(Category category)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"INSERT INTO ""Category"" (""CategoryName"", ""Description"", ""Active"") 
                              VALUES (@CategoryName, @Description, @Active);";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@Description", category.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Active", category.Active);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Update an Existing Category
        public async Task UpdateAsync(Category category)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"UPDATE ""Category"" 
                              SET ""CategoryName"" = @CategoryName, 
                                  ""Description"" = @Description, 
                                  ""Active"" = @Active
                              WHERE ""CategoryID"" = @CategoryID;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@Description", category.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Active", category.Active);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Delete a Category
        public async Task DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"DELETE FROM ""Category"" WHERE ""CategoryID"" = @CategoryID;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryID", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}