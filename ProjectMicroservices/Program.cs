using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectMicroservices.API;
using ProjectMicroservices.Model;
using ProjectMicroservices.Model.DataAcccess;
using ProjectMicroservices.Services.Authentication;
using ProjectMicroservices.Services.Repository;
using ProjectMicroservices.Services.Repository.Classes;
using ProjectMicroservices.Services.Repository.Interfaces;
using ProjectMicroservices.Services.Validation;
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

// Add db context with connection string exposed by docker, this is scoped so the connection string
// will have to be retrieved every time a new HTTP request is done
builder.Services.AddDbContext<MovieDbContext>(
    options => options.UseNpgsql(
        Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
        )
    );

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<MovieDbContext>();

// This is going to create a new instance of ProductRepository for every
// HTTP requests, giving each request their own instance to work with for database handling tasks
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

// Service for API validation
builder.Services.AddScoped<IApiKeyVal, ApiKeyVal>();

builder.Services.AddValidatorsFromAssemblyContaining(typeof(ValidateIdFromObj));

builder.Services.AddAuthorization();

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

app.MapIdentityApi<IdentityUser>();

// This statement will update all the migration I have inside the migration
// This is because the docker setup automatically creates an empty postgres db in another container
DBSetup.Setup(app);

// I defined the api in another file as an extention of app to make it more organized, as well 
// as creating map group for easier management and also cleaner interface
app.MapGroup("/api/movies")
    .InitRouteMovieAPI()
    .WithTags("Movies");



app.Run();
