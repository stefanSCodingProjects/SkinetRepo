using System;
using Core.Entities;
using Infrastructure_.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_.Data;

// Db Context comes from Microsoft Entity Framework
// In that class which uses a primary constructor, you need to pass SQL server connection string
public class StoreContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<Product> Products {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // configuration coming from ProductConfiguration.cs
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
    }
}
