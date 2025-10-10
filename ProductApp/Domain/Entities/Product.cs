namespace ProductApp.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public decimal? DiscountPrice { get; private set; }
    public string? ImageUrl { get; private set; }

    public Product(){}
    
    public Product(string name, string description, decimal price, string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));

        if (price <= 0)
            throw new ArgumentException("Price cannot be less than zero.", nameof(price));

        Name = name;
        Description = description;
        Price = price;
        ImageUrl = imageUrl;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Price cannot be less than zero.", nameof(newPrice));
        Price = newPrice;
    }

    public void ApplyDiscount(decimal discountPrice)
    {
        if (discountPrice <= 0 || discountPrice >= Price)
            throw new ArgumentException("Discount price cannot be less than zero and minor than current price.", nameof(discountPrice));
        DiscountPrice = discountPrice;
    }
    public void UpdateDetails(string name, string description, string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or whitespace.", nameof(description));
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
    }

    public void RemoveDiscount()
    {
        DiscountPrice = null;
    }
}