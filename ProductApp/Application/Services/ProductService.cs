using ProductApp.Application.DTOs;
using ProductApp.Application.Services.Interface;
using ProductApp.Domain.Entities;
using ProductApp.Domain.Repositories;

namespace ProductApp.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }
    
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<int> CreateProductAsync(CreateProductDto productDto)
    {
        ValidatePrice(productDto.Price);
        ValidateDiscountedPrice(productDto.Price, productDto.DiscountPrice);

        var product = new Product(
            productDto.Name,
            productDto.Description,
            productDto.Price,
            productDto.ImageUrl
        );

        if (productDto.DiscountPrice.HasValue)
        {
            product.ApplyDiscount(productDto.DiscountPrice.Value);
        }

        await _productRepository.AddAsync(product);
        return product.Id;
    }

    public async Task UpdateProductAsync(int id, UpdateProductDto productDto)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
            throw new ArgumentException($"Product with ID {id} not found");

        ValidatePrice(productDto.Price);
        ValidateDiscountedPrice(productDto.Price, productDto.DiscountPrice);

        existingProduct.UpdateDetails(productDto.Name, productDto.Description, productDto.ImageUrl);
        existingProduct.UpdatePrice(productDto.Price);

        
        if (productDto.DiscountPrice.HasValue)
            existingProduct.ApplyDiscount(productDto.DiscountPrice.Value);
        else
            existingProduct.RemoveDiscount();
        
        await _productRepository.UpdateAsync(existingProduct);
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new ArgumentException($"Product with ID {id} not found");

        await _productRepository.DeleteAsync(id);
    }

    public async Task UpdateProductPriceAsync(int id, decimal newPrice)
    {
        ValidatePrice(newPrice);
        
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new ArgumentException($"Product with ID {id} not found");

        product.UpdatePrice(newPrice);
        await _productRepository.UpdateAsync(product);
    }

    private static void ValidatePrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero");
    }

    private static void ValidateDiscountedPrice(decimal price, decimal? discountedPrice)
    {
        if (!discountedPrice.HasValue)
            return;

        if (discountedPrice.Value <= 0)
            throw new ArgumentException("Discounted price must be greater than zero");

        if (discountedPrice.Value >= price)
            throw new ArgumentException("Discounted price must be less than regular price");
    }
}