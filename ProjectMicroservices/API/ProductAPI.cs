namespace ProjectMicroservices.API;

public static class ProductAPI
{
    public static void AddProduct(this WebApplication app)
    {
        app.MapPost("/api/add", () =>
        {
            
        });
    }
}