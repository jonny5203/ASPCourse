using Microsoft.EntityFrameworkCore;
using ProjectMicroservices.API;
using ProjectMicroservices.Model;
using ProjectMicroservices.Services.Authentication;
using ProjectMicroservices.Services.Repository;
using ProjectMicroservices.Util;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // Listen on all network interfaces on port 8080
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add db context with connection string exposed by docker
builder.Services.AddDbContext<ProductDbContext>(
    options => options.UseNpgsql(
        Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
        )
    );

// This is going to create a new instance of ProductRepository for every
// HTTP requests, giving each request their own instance to work with for database handling tasks
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IApiKeyVal, ApiKeyVal>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// If you want to display this swagger, remove the if, as the dockerfile spesify release build
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Adding the api key authentication into the pipeline as middleware
app.UseMiddleware<ApiKeyMiddleware>();

// This statement will update all the migration I have inside the migration
// This is because the docker setup automatically creates an empty postgres db in another container
DBSetup.Setup(app);

// I defined the api in another file as an extention of app to make it more organized
app.ProductGetAPI();
app.ProductPostAPI();
app.ProductUpdateAPI();
app.ProductDeleteAPI();

app.Run();
