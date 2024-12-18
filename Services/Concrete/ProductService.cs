using Assignment.Data.Repositories;
using Assignment.Models;
using Assignment.Services.Abstract;

namespace Assignment.Services.Concrete;

public class ProductService : IProductService
{
    private readonly ProductRepository _productRepository;

    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return _productRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _productRepository.GetProductsByCategoryAsync(categoryId);
    }

    public Task<Product> GetProductByIdAsync(int id)
    {
        return _productRepository.GetByIdAsync(id);
    }

    public Task AddProductAsync(Product product)
    {
        return _productRepository.AddAsync(product);
    }

    public Task UpdateProductAsync(Product product)
    {
        return _productRepository.UpdateAsync(product);
    }

    public Task DeleteProductAsync(int id)
    {
        return _productRepository.DeleteAsync(id);
    }
}