using Assignment.Models;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Assignment.Data.Repositories
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Get All Products
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = new List<Product>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT ""ProductID"", ""SKU"", ""ProductName"", ""SupplierID"", ""CategoryID"", 
                              ""UnitPrice"", ""UnitsInStock"", ""ProductDescription"", ""ProductAvailable"" 
                              FROM ""Products"";";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                SKU = reader.GetString(reader.GetOrdinal("SKU")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                SupplierID = reader["SupplierID"] as int?,
                                CategoryID = reader["CategoryID"] as int?,
                                UnitPrice = reader["UnitPrice"] as decimal?,
                                UnitsInStock = reader["UnitsInStock"] as int?,
                                ProductDescription = reader["ProductDescription"] as string,
                                ProductAvailable = reader.GetBoolean(reader.GetOrdinal("ProductAvailable"))
                            });
                        }
                    }
                }
            }

            return products;
        }
        
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = new List<Product>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT ""ProductID"", ""ProductName"", ""ProductDescription"", ""UnitPrice"", ""Picture"", ""CategoryID"" 
                              FROM ""Products"" WHERE ""CategoryID"" = @CategoryID;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryID", categoryId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                ProductDescription = reader["ProductDescription"] as string,
                                UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                                Picture = reader["Picture"] as string,
                                CategoryID = reader["CategoryID"] as int?
                            });
                        }
                    }
                }
            }

            return products;
        }

        // Get Product by ID
        public async Task<Product> GetByIdAsync(int id)
        {
            Product product = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT ""ProductID"", ""SKU"", ""ProductName"", ""SupplierID"", ""CategoryID"", 
                              ""UnitPrice"", ""UnitsInStock"", ""ProductDescription"", ""ProductAvailable"" 
                              FROM ""Products"" WHERE ""ProductID"" = @ProductID;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            product = new Product
                            {
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                SKU = reader.GetString(reader.GetOrdinal("SKU")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                SupplierID = reader["SupplierID"] as int?,
                                CategoryID = reader["CategoryID"] as int?,
                                UnitPrice = reader["UnitPrice"] as decimal?,
                                UnitsInStock = reader["UnitsInStock"] as int?,
                                ProductDescription = reader["ProductDescription"] as string,
                                ProductAvailable = reader.GetBoolean(reader.GetOrdinal("ProductAvailable"))
                            };
                        }
                    }
                }
            }

            return product;
        }

        // Add Product
        public async Task AddAsync(Product product)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"INSERT INTO ""Products"" 
                              (""SKU"", ""ProductName"", ""SupplierID"", ""CategoryID"", ""UnitPrice"", 
                               ""UnitsInStock"", ""ProductDescription"", ""ProductAvailable"") 
                              VALUES (@SKU, @ProductName, @SupplierID, @CategoryID, @UnitPrice, 
                                      @UnitsInStock, @ProductDescription, @ProductAvailable);";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SKU", product.SKU);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@SupplierID", product.SupplierID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CategoryID", product.CategoryID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UnitsInStock", product.UnitsInStock ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ProductDescription", product.ProductDescription ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ProductAvailable", product.ProductAvailable);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Update Product
        public async Task UpdateAsync(Product product)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"UPDATE ""Products"" 
                              SET ""SKU"" = @SKU, ""ProductName"" = @ProductName, ""SupplierID"" = @SupplierID, 
                                  ""CategoryID"" = @CategoryID, ""UnitPrice"" = @UnitPrice, 
                                  ""UnitsInStock"" = @UnitsInStock, ""ProductDescription"" = @ProductDescription, 
                                  ""ProductAvailable"" = @ProductAvailable
                              WHERE ""ProductID"" = @ProductID;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);
                    command.Parameters.AddWithValue("@SKU", product.SKU);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@SupplierID", product.SupplierID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CategoryID", product.CategoryID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UnitsInStock", product.UnitsInStock ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ProductDescription", product.ProductDescription ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ProductAvailable", product.ProductAvailable);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Delete Product
        public async Task DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"DELETE FROM ""Products"" WHERE ""ProductID"" = @ProductID;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}