namespace ProductApi.Models;

public class Product 
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
}