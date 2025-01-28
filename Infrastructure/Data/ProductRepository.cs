using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    private readonly StoreContext context = context;

    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await context.Products.Select(x => x.Brand)
        .Distinct()
        .ToListAsync();
    }

     public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await context.Products.Select(x => x.Type)
        .Distinct()
        .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        var query = context.Products.AsQueryable();

        if(!string.IsNullOrWhiteSpace(brand))
         query = query.Where(x => x.Brand == brand);

        if(!string.IsNullOrWhiteSpace(type))
         query = query.Where(x => x.Type == type);

        query = sort switch
        {
            "priceAsc" => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            _ => query.OrderBy(x => x.Name)
        };
        
        return await query.ToListAsync();
    }

    public bool ProductExists(int id)
    {
        return context.Products.Any(product => product.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
       return await context.SaveChangesAsync() > 0;
       // this method saves all changes made to the db - if any changes is greater than 1
       // therefore since changes made are greater than 1, 1>0 so true
    }

    public void UpdateProduct(Product product)
    {
        context.Entry(product).State = EntityState.Modified;
    }
}
