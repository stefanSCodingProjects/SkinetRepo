using Core.Interfaces;
using Infrastructure_.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// adding services for the Db Connection
builder.Services.AddDbContext<StoreContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    // using means that every code that uses "scope"- once the framework is finished using it, 
    // it will dispose of any services we have used, that's because it's outside of dependency injection.
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>(); // method is referred to service locator pattern
    await context.Database.MigrateAsync(); // creates the db if it doesn't exist & applies pending migrations
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.Run();
