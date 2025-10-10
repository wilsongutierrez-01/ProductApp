using ProductApp.Application.DTOs;
using ProductApp.Domain.Entities;

namespace ProductApp.Application.Services.Interface;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<int> CreateProductAsync(CreateProductDto productDto);
    Task UpdateProductAsync(int id, UpdateProductDto productDto);
    Task DeleteProductAsync(int id);
    Task UpdateProductPriceAsync(int id, decimal newPrice);
}