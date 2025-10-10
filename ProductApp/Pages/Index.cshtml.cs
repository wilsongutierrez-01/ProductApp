using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductApp.Application.DTOs;
using ProductApp.Application.Services;
using ProductApp.Application.Services.Interface;
using ProductApp.Domain.Entities;

namespace ProductApp.Pages;

public class IndexModel : PageModel
{
    private readonly IProductService _productService;
    
    public IndexModel(IProductService productService)
    {
        _productService = productService;
    }

    [BindProperty]
    public IEnumerable<Product>? Products { get; set; }
    
    [TempData]
    public string? Message { get; set; }
    
    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            Products = await _productService.GetAllProductsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al cargar productos: {ex.Message}";
        }
    }

    public async Task<IActionResult> OnPostCreateAsync(string name, string description, decimal price, 
        string imageUrl, decimal? discountPrice)
    {
        try
        {
            var productDto = new CreateProductDto
            {
                Name = name,
                Description = description,
                Price = price,
                ImageUrl = imageUrl,
                DiscountPrice = discountPrice
            };

            var productId = await _productService.CreateProductAsync(productDto);
            Message = $"Producto '{name}' agregado correctamente (ID: {productId})";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al crear producto: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAsync(int id, string name, string description, decimal price, 
        string imageUrl, decimal? discountPrice)
    {
        try
        {
            var updateDto = new UpdateProductDto
            {
                Name = name,
                Description = description,
                Price = price,
                ImageUrl = imageUrl,
                DiscountPrice = discountPrice
            };

            await _productService.UpdateProductAsync(id, updateDto);
            Message = $"Producto '{name}' actualizado correctamente";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al actualizar producto: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
            Message = "Producto eliminado correctamente";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al eliminar producto: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdatePriceAsync(int id, decimal newPrice)
    {
        try
        {
            await _productService.UpdateProductPriceAsync(id, newPrice);
            Message = "Precio actualizado correctamente";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al actualizar precio: {ex.Message}";
        }

        return RedirectToPage();
    }
}