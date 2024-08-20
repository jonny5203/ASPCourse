using ProjectMicroservices.Model;
using ProjectMicroservices.Services.Repository;

namespace ProjectMicroservices.API;

// This class serves as a container for the different static extension classes for WepApplication class, aka. the app variable in program.cs
// 
public static class ProductAPI
{
    public static void ProductGetAPI(this WebApplication app)
    {
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
    }

    public static void ProductPostAPI(this WebApplication app)
    {
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
    }

    public static void ProductDeleteAPI(this WebApplication app)
    {
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
    }

    public static void ProductUpdateAPI(this WebApplication app)
    {
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
    }
}