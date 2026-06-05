namespace ProductApi.Models;

// Models/Product.cs
public class Product
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}