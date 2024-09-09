using Microsoft.EntityFrameworkCore;
using ProjectMicroservices.Model.DataAcccess;

namespace ProjectMicroservices.Util;

public static class DBSetup
{
    public static void Setup(WebApplication app)
    {
        // This statement will update all the migration I have inside the migration
        // This is because the docker setup automatically creates an empty postgres db in another container
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<MovieDbContext>();  

            context.Database.Migrate(); // This line triggers the migration process
        }
    }
}