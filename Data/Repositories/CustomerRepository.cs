using Assignment.Models;

namespace Assignment.Data.Repositories;

public class CustomerRepository : GenericRepository<Customer>
{
    public CustomerRepository(string connectionString) : base(connectionString)
    {
    }
}