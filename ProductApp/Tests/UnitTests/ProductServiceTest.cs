using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProductApp.Application.DTOs;
using ProductApp.Application.Services;
using ProductApp.Domain.Entities;
using ProductApp.Infrastructure.Data;
using ProductApp.Infrastructure.Repositories;
using Xunit;

namespace ProductApp.Tests.UnitTests;

public class ProductServiceTestSqlServer : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ProductService _productService;
    private readonly ProductRepository _productRepository;

    public ProductServiceTestSqlServer()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=localhost,1435;Database=ProductAppDbTest;User Id=sa;Password=root12345@Password;TrustServerCertificate=True;")
            .Options;

        _context = new AppDbContext(options);

        _context.Database.Migrate();

        _productRepository = new ProductRepository(_context);
        _productService = new ProductService(_productRepository);

        _context.Products.RemoveRange(_context.Products);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnAllProducts()
    {
        var expectedProducts = new List<Product>
        {
            new Product("Product 1", "Description 1", 10.99m, "image1.jpg"),
            new Product("Product 2", "Description 2", 20.99m, "image2.jpg"),
        };

        await _context.Products.AddRangeAsync(expectedProducts);
        await _context.SaveChangesAsync();

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedProducts, options => options.Excluding(p => p.Id));
    }

    [Fact]
    public async Task CreateProductAsync_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        var productDto = new CreateProductDto
        {
            Name = "New Product",
            Description = "Test Description",
            Price = 25.99m,
            ImageUrl = "test.jpg",
            DiscountPrice = 20.99m
        };

        var newProductId = await _productService.CreateProductAsync(productDto);
        var createdProduct = await _context.Products.FindAsync(newProductId);

        createdProduct.Should().NotBeNull();
        createdProduct!.Name.Should().Be(productDto.Name);
        createdProduct.Price.Should().Be(productDto.Price);
    }

    [Fact]
    public async Task CreateProductAsync_WithInvalidPrice_ShouldThrowException()
    {
        var productDto = new CreateProductDto
        {
            Name = "Invalid Product",
            Description = "Test Description",
            Price = 0, // Invalid price
            ImageUrl = "test.jpg"
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _productService.CreateProductAsync(productDto));

        (await _context.Products.AnyAsync(p => p.Name == productDto.Name)).Should().BeFalse();
    }

    [Fact]
    public async Task UpdateProductPriceAsync_WithValidPrice_ShouldUpdatePrice()
    {
        var existingProduct = new Product("Test Product", "Test Description", 10.00m);
        await _context.Products.AddAsync(existingProduct);
        await _context.SaveChangesAsync();

        var newPrice = 15.00m;

        await _productService.UpdateProductPriceAsync(existingProduct.Id, newPrice);

        var updatedProduct = await _context.Products.FindAsync(existingProduct.Id);

        updatedProduct!.Price.Should().Be(newPrice);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}