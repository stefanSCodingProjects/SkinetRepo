using System;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure_.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // This piece of code did exactly what migration did for us, 
        // but not at least we won't get the warning. 
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        // for example if you wanted to specify that a field cannot be NULL, you can write
        // builder.Property(x => x.Name).IsRequired();
    }
}
