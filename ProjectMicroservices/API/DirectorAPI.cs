using Microsoft.AspNetCore.Authorization;
using ProjectMicroservices.Model;
using ProjectMicroservices.Services.Repository.Interfaces;
using ProjectMicroservices.Services.Repository.UnitOfWork.Interface;
using ProjectMicroservices.Services.Validation;
using System.ComponentModel.DataAnnotations;

namespace ProjectMicroservices.API;

public static class DirectorAPI
{
    public static RouteGroupBuilder InitRouteDirectorAPI(this RouteGroupBuilder group)
    {
        DirectorGetAPI(group);
        DirectorUpsertAPI(group);
        DirectorDeleteAPI(group);

        return group;
    }
    private static void DirectorGetAPI(RouteGroupBuilder group)
    {
        // Testing Identity with web API
        group.MapGet("/api", [Authorize] (IDirectorRepository directorRepository) =>
        {
            return Results.Ok();
        });

        group.MapGet("/api/test", (IUnitOfWork unitOfWork) =>
        {
            var director = unitOfWork.Directors.GetAll;
            return Results.Ok();
        });

        // Get movie based on id, and validate if Id is valid number
        group.MapGet("/id", (IUnitOfWork unitOfWork, [Required, Range(0, int.MaxValue)] int id) =>
        {
            try
            {
                var director = unitOfWork.Directors.Get(id);
                return Results.Ok(director);
            }
            catch
            {
                return Results.Json(new { message = "No entry exists on director ID provided" }, statusCode: StatusCodes.Status404NotFound);
            }
        });

        group.MapGet("/all", (IUnitOfWork unitOfWork) =>
        {
            try
            {
                var directors = unitOfWork.Directors.GetAll();
                return Results.Ok(directors);
            }
            catch
            {
                return Results.Json(new { message = "No entry exists in directors table" }, statusCode: StatusCodes.Status404NotFound);
            }
        });
    }

    private static void DirectorUpsertAPI(RouteGroupBuilder group)
    {

        // Adding product to db, by getting repo through DI, and deserialize
        // json object into a Product object, and ensure that productid is not provided
        group.MapPost("", (IUnitOfWork unitOfWork, Director director) =>
        {
            // setting id to 0 so that ef core will recognize that this should be
            // an autoincremented value
            director.Id = 0;

            try
            {
                unitOfWork.Directors.Add(director);
                unitOfWork.SaveChanges();
                return Results.Ok(new
                {
                    status = "Successfully Added Movie to List!",
                    createdItem = director
                });
            }
            catch
            {
                return Results.Json(new { message = "Something went wrong, could not add movie to database" }, statusCode: StatusCodes.Status500InternalServerError);
            }
        }).AddEndpointFilter<ValidationFilter<Movie>>();

        // update an existing product in the database with new values
        group.MapPut("", (IUnitOfWork unitOfWork, Director director) =>
        {
            if (director.Id is 0 or < 0)
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }

            try
            {
                unitOfWork.Directors.Update(director);
                unitOfWork.SaveChanges();
                return Results.Ok(new
                {
                    message = "Successfully Updated Movie!",
                    updatedItem = director
                });
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }).AddEndpointFilter<ValidationFilter<Movie>>();

        // TODO: a method to add multiple movies at a time
    }

    private static void DirectorDeleteAPI(RouteGroupBuilder group)
    {
        // Delete a product based on provided id in parameter from db
        group.MapDelete("", (IUnitOfWork unitOfWork, int Id) =>
        {
            try
            {
                var deleted = unitOfWork.Directors.Delete(Id);
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