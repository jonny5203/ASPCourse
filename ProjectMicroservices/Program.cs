

using Microsoft.EntityFrameworkCore;
using ProjectMicroservices.API;
using ProjectMicroservices.Model;
using ProjectMicroservices.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // Listen on all network interfaces on port 8080
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductDbContext>(
    options => options.UseNpgsql(
        Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
        )
    );

builder.Services.AddTransient<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// If you want to display this swagger, remove the if, as the dockerfile spesify release build
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// This statement will update all the migration I have inside the migration
// This is because the docker setup automatically creates an empty postgres db in another container
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ProductDbContext>();  

    context.Database.Migrate(); // This line triggers the migration process
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/product", (IProductRepository productRepository, int productId) =>
{
    if (int.IsNegative(productId))
    {
        return Results.StatusCode(StatusCodes.Status400BadRequest);
    }
    
    try
    {
        productRepository.GetProductByID(productId);
        return Results.Ok(productId);
    }
    catch (Exception ex)
    {
        return Results.StatusCode(StatusCodes.Status404NotFound);
    }
});

app.MapGet("/product/all", (IProductRepository productRepository) =>
{
    try
    {
        IEnumerable<Product> products = productRepository.getProducts();
        return Results.Ok(products);
        
    }
    catch (Exception ex)
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

// Adding product to db, by getting repo through DI, and deserialize
// json object into a Product object, and ensure that productid is not provided
app.MapPost("/product", (IProductRepository productRepository, Product product) =>
{
    // setting id to 0 so that ef core will recognize that this should be
    // an autoincremented value
    product.Id = 0;
    
    try
    {
        productRepository.InsertProduct(product);
        productRepository.Save();
        return Results.Created($"/product/{product.Id}", product);
    }
    catch
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

// update an existing product in the database with new values
app.MapPut("/product", (IProductRepository productRepository, Product product) =>
{
    if (product.Id is 0 or < 0)
    {
        return Results.StatusCode(StatusCodes.Status400BadRequest);
    }
    
    try
    {
        productRepository.UpdateProduct(product);
        productRepository.Save();
        return Results.StatusCode(StatusCodes.Status200OK);
    }
    catch
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

// Delete a product based on provided id in parameter from db
app.MapDelete("/product", (IProductRepository productRepository, int productId) =>
{
    try
    {
        productRepository.DeleteProduct(productId);
        productRepository.Save();
        return Results.StatusCode(StatusCodes.Status200OK);
    }
    catch
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

app.Run();
