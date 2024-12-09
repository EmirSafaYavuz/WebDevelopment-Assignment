using Assignment.Data.Repositories;
using Assignment.Models;
using Assignment.Services.Abstract;

namespace Assignment.Services.Concrete;

public class CustomerService : ICustomerService
{
    private readonly GenericRepository<Customer> _customerRepository;

    public CustomerService(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        _customerRepository = new GenericRepository<Customer>(connectionString);
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        await _customerRepository.AddAsync(customer);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        await _customerRepository.UpdateAsync(customer);
    }

    public async Task DeleteCustomerAsync(int id)
    {
        await _customerRepository.DeleteAsync(id);
    }
}