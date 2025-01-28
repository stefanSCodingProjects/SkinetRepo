using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure_.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if(!context.Products.Any())
        {
            // Read the JSON
            var productsData = await File.ReadAllTextAsync(@"C:\Users\stefa\OneDrive\Desktop\.NET Angular Ecommerce Shop\Infrastructure\Data\SeedData\products.json");

            Console.WriteLine($"Raw JSON date {productsData}");
            // Deserialize
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);

            if(products == null) return;

            context.Products.AddRange(products);

            await context.SaveChangesAsync();
        }
    }
}
