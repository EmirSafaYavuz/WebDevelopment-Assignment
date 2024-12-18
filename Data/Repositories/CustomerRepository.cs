using Assignment.Models;
using Npgsql; // PostgreSQL için Npgsql kütüphanesini kullanıyoruz.
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Assignment.Data.Repositories
{
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Get All Customers
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var customers = new List<Customer>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT ""CustomerID"", ""FirstName"", ""LastName"", ""Email"", ""Phone"", 
                              ""Address1"", ""Address2"", ""City"", ""State"", ""PostalCode"", ""Country"", ""IsActive""
                              FROM ""Customer"";";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            customers.Add(new Customer
                            {
                                CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader["Phone"] as string,
                                Address1 = reader["Address1"] as string,
                                Address2 = reader["Address2"] as string,
                                City = reader["City"] as string,
                                State = reader["State"] as string,
                                PostalCode = reader["PostalCode"] as string,
                                Country = reader["Country"] as string,
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            });
                        }
                    }
                }
            }

            return customers;
        }

        // Get Customer by ID
        public async Task<Customer> GetByIdAsync(int id)
        {
            Customer customer = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT ""CustomerID"", ""FirstName"", ""LastName"", ""Email"", ""Phone"", 
                              ""Address1"", ""Address2"", ""City"", ""State"", ""PostalCode"", ""Country"", ""IsActive""
                              FROM ""Customer"" WHERE ""CustomerID"" = @CustomerID;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            customer = new Customer
                            {
                                CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader["Phone"] as string,
                                Address1 = reader["Address1"] as string,
                                Address2 = reader["Address2"] as string,
                                City = reader["City"] as string,
                                State = reader["State"] as string,
                                PostalCode = reader["PostalCode"] as string,
                                Country = reader["Country"] as string,
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                    }
                }
            }

            return customer;
        }

        // Add Customer
        public async Task AddAsync(Customer customer)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"INSERT INTO ""Customer"" 
                              (""FirstName"", ""LastName"", ""Email"", ""Phone"", ""Address1"", ""Address2"", ""City"", ""State"", ""PostalCode"", ""Country"", ""IsActive"") 
                              VALUES (@FirstName, @LastName, @Email, @Phone, @Address1, @Address2, @City, @State, @PostalCode, @Country, @IsActive);";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@Phone", customer.Phone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address1", customer.Address1 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address2", customer.Address2 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@State", customer.State ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PostalCode", customer.PostalCode ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Country", customer.Country ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", customer.IsActive);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Update Customer
        public async Task UpdateAsync(Customer customer)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"UPDATE ""Customer"" 
                              SET ""FirstName"" = @FirstName, ""LastName"" = @LastName, ""Email"" = @Email, 
                                  ""Phone"" = @Phone, ""Address1"" = @Address1, ""Address2"" = @Address2, 
                                  ""City"" = @City, ""State"" = @State, ""PostalCode"" = @PostalCode, 
                                  ""Country"" = @Country, ""IsActive"" = @IsActive
                              WHERE ""CustomerID"" = @CustomerID;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@Phone", customer.Phone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address1", customer.Address1 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address2", customer.Address2 ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@State", customer.State ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PostalCode", customer.PostalCode ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Country", customer.Country ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", customer.IsActive);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Delete Customer
        public async Task DeleteAsync(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"DELETE FROM ""Customer"" WHERE ""CustomerID"" = @CustomerID;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}