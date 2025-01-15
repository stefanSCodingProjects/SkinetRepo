using System;
using Core.Entities;
using Infrastructure_.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

// attributes assigned
[ApiController] 
// this is how the server knows where to send the incoming HTTP request
// the usage of controller is a placeholder for the name of controller minus the word controller
// so basically api/productscontroller - (controller) = api/products 
[Route("api/[controller]")] 

public class ProductsController : ControllerBase
{
    private readonly StoreContext context;

    public ProductsController(StoreContext context)
    {
        this.context = context;
    }

    // HTTP Requests
    [HttpGet]
    // async response because of HTTP
    // returns a Task (allowing us to await in the method we are about to create)
    // ActionResult allows us to return HTTP type of responses (in that case IEnumerable List of Product)
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        // if we want to return a list of products we need to get them from our database
        // this means we need to inject the StoreContext.cs into our controller
        return await context.Products.ToListAsync();
    }

    // Find that specific product in the databse
    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if(product == null) return NotFound();
        
        return product;
    }

    // Allow us to create a product in our database
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        context.Entry(product).State = EntityState.Modified;

        await context.SaveChangesAsync();
        
        return NoContent();
    }

    private bool ProductExists(int id)
    {
        return context.Products.Any(x => x.Id == id);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null) return NotFound();

        // else
        context.Products.Remove(product);

        await context.SaveChangesAsync();

        return NoContent();
    }
}
