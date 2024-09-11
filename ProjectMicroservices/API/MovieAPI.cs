using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectMicroservices.Model;
using ProjectMicroservices.Services.Repository;
using ProjectMicroservices.Services.Repository.Classes;
using ProjectMicroservices.Services.Repository.Interfaces;
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
        
        // Get movie based on id, and validate if Id is valid number
        group.MapGet("", (IMovieRepository movieRepository, [Required, Range(0, int.MaxValue)] int movieId) =>
        {
            try
            {
                var movie = movieRepository.Get(movieId);
                return Results.Ok(movie);
            }
            catch (Exception ex)
            {
                return Results.Json(new { message = "No entry exists on ID provided"}, statusCode: StatusCodes.Status404NotFound);;
            }
        });

        group.MapGet("/all", (IMovieRepository movieRepository) =>
        {
            try
            {
                var products = movieRepository.GetAll();
                return Results.Ok(products);
            }
            catch (Exception ex)
            {
                return Results.Json(new { message = "No entry exists"}, statusCode: StatusCodes.Status404NotFound);
            }
        });
    }

    private static void MovieUpsertAPI(RouteGroupBuilder group)
    {
        
        // Adding product to db, by getting repo through DI, and deserialize
        // json object into a Product object, and ensure that productid is not provided
        group.MapPost("", (IMovieRepository movieRepository, Movie movie) =>
        {
            // setting id to 0 so that ef core will recognize that this should be
            // an autoincremented value
            movie.Id = 0;

            try
            {
                movieRepository.Add(movie);
                movieRepository.SaveChanges();
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
        group.MapPut("", (IMovieRepository movieRepository, Movie movie) =>
        {
            if (movie.Id is 0 or < 0)
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }

            try
            {
                movieRepository.Update(movie);
                movieRepository.SaveChanges();
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
        group.MapDelete("", (IMovieRepository movieRepository, int movieId) =>
        {
            try
            {
                var deletedMovie = movieRepository.Delete(movieId);
                if(deletedMovie == null) return Results.NotFound();
                
                movieRepository.SaveChanges();
                
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