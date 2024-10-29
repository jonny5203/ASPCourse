using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectMicroservices.Model;
using ProjectMicroservices.Services.Repository;
using ProjectMicroservices.Services.Repository.Classes;
using ProjectMicroservices.Services.Repository.Interfaces;
using ProjectMicroservices.Services.Repository.UnitOfWork.Interface;
using ProjectMicroservices.Services.Validation;

namespace ProjectMicroservices.API;

// This class serves as a container for the different static extension classes for WepApplication class, aka. the app variable in program.cs
// 
public static class MovieAPI
{
    public static RouteGroupBuilder InitRouteMovieAPI(this RouteGroupBuilder group)
    {
        MovieGetAPI(group);
        MovieUpsertAPI(group);
        MovieDeleteAPI(group);
        
        return group;
    }
    private static void MovieGetAPI(RouteGroupBuilder group)
    {
        // Testing Identity with web API
        group.MapGet("/api", [Authorize] (IMovieRepository movieRepository) =>
        {
            return Results.Ok();
        });
        
        group.MapGet("/api/test", (IUnitOfWork unitOfWork) =>
        {
            var movies = unitOfWork.Movies.GetAll;
            return Results.Ok();
        });
        
        // Get movie based on id, and validate if Id is valid number
        group.MapGet("/id", (IUnitOfWork unitOfWork, [Required, Range(0, int.MaxValue)] int id) =>
        {
            try
            {
                var movie = unitOfWork.Movies.Get(id);
                return Results.Ok(movie);
            }
            catch
            {
                return Results.Json(new { message = "No entry exists on moview ID provided"}, statusCode: StatusCodes.Status404NotFound);
            }
        });

        group.MapGet("/all", (IUnitOfWork unitOfWork) =>
        {
            try
            {
                var movies = unitOfWork.Movies.GetAll();
                return Results.Ok(movies);
            }
            catch
            {
                return Results.Json(new { message = "No entry exists in movie table"}, statusCode: StatusCodes.Status404NotFound);
            }
        });
    }

    private static void MovieUpsertAPI(RouteGroupBuilder group)
    {
        
        // Adding product to db, by getting repo through DI, and deserialize
        // json object into a Product object, and ensure that productid is not provided
        group.MapPost("", (IUnitOfWork unitOfWork, Movie movie) =>
        {
            // setting id to 0 so that ef core will recognize that this should be
            // an autoincremented value
            movie.Id = 0;

            try
            {
                unitOfWork.Movies.Add(movie);
                unitOfWork.Movies.SaveChanges();
                return Results.Ok(new
                {
                    status = "Successfully Added Movie to List!",
                    createdItem = movie
                });
            }
            catch
            {
                return Results.Json(new{ message = "Something went wrong, could not add movie to database" }, statusCode: StatusCodes.Status500InternalServerError);
            }
        }).AddEndpointFilter<ValidationFilter<Movie>>();
        
        // update an existing product in the database with new values
        group.MapPut("", (IUnitOfWork unitOfWork, Movie movie) =>
        {
            if (movie.Id is 0 or < 0)
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }

            try
            {
                unitOfWork.Movies.Update(movie);
                unitOfWork.Movies.SaveChanges();
                return Results.Ok(new { 
                    message = "Successfully Updated Movie!", 
                    updatedItem = movie
                });
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }).AddEndpointFilter<ValidationFilter<Movie>>();
        
        // TODO: a method to add multiple movies at a time
    }

    private static void MovieDeleteAPI(RouteGroupBuilder group)
    {
        // Delete a product based on provided id in parameter from db
        group.MapDelete("", (IUnitOfWork unitOfWork, int movieId) =>
        {
            try
            {
                var deletedMovie = unitOfWork.Movies.Delete(movieId);
                if(deletedMovie == null) return Results.NotFound();
                
                unitOfWork.Movies.SaveChanges();
                
                return Results.Ok(new { 
                    message = "Successfully Added Movie to List!", 
                    deletedItem = deletedMovie
                });
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        });
    }
}