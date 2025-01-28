using System;
using System.Text;

namespace Core.Entities;

public class Product : BaseEntity
{
    // required makes sure that the field is populated before you can create an object
    // remember you access the Id or Name ... etc by ClassName.Property ==> Product.Id, Product.Name...
    
    // Id comes from inherited BaseEntity 
    public required string Name { get; set; }
    public required string  Description { get; set; }
    public required decimal Price { get; set; }
    public required string PictureUrl { get; set; }
    public required string Type { get; set; }
    public required string Brand { get; set; }
    public required int QuantityInStock { get; set; }
}
