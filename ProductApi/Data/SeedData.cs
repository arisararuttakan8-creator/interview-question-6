using ProductApi.Models;

namespace ProductApi.Data;

public static class SeedData
{
    public static async Task Initialize(AppDbContext context)
    {
        if (context.Categories.Any()) return; // ถ้ามีข้อมูลแล้วไม่ต้อง seed ซ้ำ

        // Categories 
        var categories = new List<Category>
        {
            new() { Name = "Beer" },
            new() { Name = "Spirit" },
            new() { Name = "Non-Alcoholic" },
            new() { Name = "Water" },
            new() { Name = "Energy Drink" }
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        // Generate product code xxxx-xxxx-xxxx-xxxx
        static string GenerateCode(Random rng)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string Part() => new string(Enumerable.Range(0, 4)
                .Select(_ => chars[rng.Next(chars.Length)]).ToArray());
            return $"{Part()}-{Part()}-{Part()}-{Part()}";
        }

        // Products 
        var productNames = new Dictionary<string, string[]>
        {
            ["Beer"] = new[] { "Chang Classic", "Chang Light", "Chang Espresso",
                "Archa Beer", "Federation", "Cheers" },
            ["Spirit"] = new[] { "Ruang Khao", "Hong Thong", "Blend 285",
                "Mekhong", "SangSom", "Regency Brandy" },
            ["Non-Alcoholic"] = new[] { "Oishi Green Tea", "Oishi Gyokuro",
                "Oishi Gold", "Oishi Chakulza", "est Cola", "est Play" },
            ["Water"] = new[] { "Crystal Water", "Crystal Soda" },
            ["Energy Drink"] = new[] { "Shark Energy", "Shark Vitamin" }
        };

        var rng = new Random();
        var products = new List<Product>();

        foreach (var category in categories)
        {
            var names = productNames[category.Name];
            int count = 100 / categories.Count; // กระจายให้ครบ 100 

            for (int i = 0; i < count; i++)
            {
                var baseName = names[i % names.Length];
                products.Add(new Product
                {
                    Code = GenerateCode(rng),
                    Name = $"{baseName} {rng.Next(100, 999)}ml",
                    CategoryId = category.Id,
                    Price = Math.Round((decimal)(rng.NextDouble() * 200 + 20), 2),
                    Stock = rng.Next(10, 500),
                    CreatedBy = "seed",
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}