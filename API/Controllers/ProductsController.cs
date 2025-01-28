using System;
using Core.Entities;
using Core.Interfaces;
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

public class ProductsController(IProductRepository irepo) : ControllerBase
{
    // HTTP Requests
    [HttpGet]
    // async response because of HTTP
    // returns a Task (allowing us to await in the method we are about to create)
    // ActionResult allows us to return HTTP type of responses (in that case IEnumerable List of Product)
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        // if we want to return a list of products we need to get them from our database
        // this means we need to inject the StoreContext.cs into our controller
        return Ok(await irepo.GetProductsAsync(brand, type, sort));
    }

    // Find that specific product in the databse
    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await irepo.GetProductByIdAsync(id);

        if(product == null) return NotFound();

        return product;
    }

    // Allow us to create a product in our database
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        irepo.AddProduct(product);

        if(await irepo.SaveChangesAsync())
        {
            return  CreatedAtAction("GetProduct", new {id = product.Id}, product);
        }
        
        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        if(await irepo.SaveChangesAsync())
        {
            return NoContent();
        }
        
        return BadRequest("Problem updating the product");

    }

    private bool ProductExists(int id)
    {
        return irepo.ProductExists(id);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await irepo.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        // else
        irepo.DeleteProduct(product);

        if(await irepo.SaveChangesAsync())
        {
            return NoContent();
        }
        
        return BadRequest("Problem deleting the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await irepo.GetBrandsAsync());
    }

     [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await irepo.GetTypesAsync());
    }
}
