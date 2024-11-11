using Microsoft.AspNetCore.Authorization;
using ProjectMicroservices.Model;
using ProjectMicroservices.Services.Repository.Interfaces;
using ProjectMicroservices.Services.Repository.UnitOfWork.Interface;
using ProjectMicroservices.Services.Validation;
using System.ComponentModel.DataAnnotations;

namespace ProjectMicroservices.API;

public static class GenreAPI
{
    public static RouteGroupBuilder InitRouteGenreAPI(this RouteGroupBuilder group)
    {
        GenreGetAPI(group);
        GenreUpsertAPI(group);
        GenreDeleteAPI(group);

        return group;
    }
    private static void GenreGetAPI(RouteGroupBuilder group)
    {
        // Testing Identity with web API
        group.MapGet("/api", [Authorize] (IGenreRepository genreRepository) =>
        {
            return Results.Ok();
        });

        group.MapGet("/api/test", (IUnitOfWork unitOfWork) =>
        {
            var genres = unitOfWork.Genres.GetAll;
            return Results.Ok();
        });

        // Get movie based on id, and validate if Id is valid number
        group.MapGet("/id", (IUnitOfWork unitOfWork, [Required, Range(0, int.MaxValue)] int id) =>
        {
            try
            {
                var genre = unitOfWork.Genres.Get(id);
                return Results.Ok(genre);
            }
            catch
            {
                return Results.Json(new { message = "No entry exists on moview ID provided" }, statusCode: StatusCodes.Status404NotFound);
            }
        });

        group.MapGet("/all", (IUnitOfWork unitOfWork) =>
        {
            try
            {
                var movies = unitOfWork.Genres.GetAll();
                return Results.Ok(movies);
            }
            catch
            {
                return Results.Json(new { message = "No entry exists in movie table" }, statusCode: StatusCodes.Status404NotFound);
            }
        });
    }

    private static void GenreUpsertAPI(RouteGroupBuilder group)
    {

        // Adding product to db, by getting repo through DI, and deserialize
        // json object into a Product object, and ensure that productid is not provided
        group.MapPost("", (IUnitOfWork unitOfWork, Genre genre) =>
        {
            // setting id to 0 so that ef core will recognize that this should be
            // an autoincremented value
            genre.Id = 0;

            try
            {
                unitOfWork.Genres.Add(genre);
                unitOfWork.SaveChanges();
                return Results.Ok(new
                {
                    status = "Successfully Added Movie to List!",
                    createdItem = genre
                });
            }
            catch
            {
                return Results.Json(new { message = "Something went wrong, could not add movie to database" }, statusCode: StatusCodes.Status500InternalServerError);
            }
        }).AddEndpointFilter<ValidationFilter<Movie>>();

        // update an existing product in the database with new values
        group.MapPut("", (IUnitOfWork unitOfWork, Genre genre) =>
        {
            if (genre.Id is 0 or < 0)
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }

            try
            {
                unitOfWork.Movies.Update(genre);
                unitOfWork.SaveChanges();
                return Results.Ok(new
                {
                    message = "Successfully Updated Movie!",
                    updatedItem = genre
                });
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }).AddEndpointFilter<ValidationFilter<Movie>>();

        // TODO: a method to add multiple movies at a time
    }

    private static void GenreDeleteAPI(RouteGroupBuilder group)
    {
        // Delete a product based on provided id in parameter from db
        group.MapDelete("", (IUnitOfWork unitOfWork, int Id) =>
        {
            try
            {
                var deleted = unitOfWork.Genres.Delete(Id);
                if (deleted == null) return Results.NotFound();

                unitOfWork.SaveChanges();

                return Results.Ok(new
                {
                    message = "Successfully Added Movie to List!",
                    deletedItem = deleted
                });
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        });
    }
}