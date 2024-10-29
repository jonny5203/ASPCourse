using ProjectMicroservices.Model;
using ProjectMicroservices.Services.Repository.UnitOfWork.Class;
using ProjectMicroservices.Services.Repository.UnitOfWork.Interface;
using ProjectMicroservices.Services.Validation;

namespace ProjectMicroservices.API;

static public class ActorAPI
{
    public static RouteGroupBuilder initActorAPI(this RouteGroupBuilder group)
    {
        ActorGetAPI(group);
        ActorUpsertAPI(group);
        ActorDeleteAPI(group);
        return group;
    }

    public static void ActorGetAPI(RouteGroupBuilder group)
    {
        group.MapGet("/get/id", (IUnitOfWork unitOfWork, int id) =>
        {
            try
            {
                var actor = unitOfWork.Actors.Get(id);
                return Results.Ok(actor);
            }
            catch
            {
                return Results.Json(new { message = "No entry exists on moview ID provided" }, statusCode: StatusCodes.Status404NotFound);
            }

        });

        group.MapGet("/get/all", (IUnitOfWork unitOfWork) =>
        {
            try
            {
                var actor = unitOfWork.Actors.GetAll();
                return Results.Ok(actor);
            }
            catch
            {
                return Results.Json(new { message = "No entry exists in movie table" }, statusCode: StatusCodes.Status404NotFound);
            }
        });
    }

    private static void ActorUpsertAPI(RouteGroupBuilder group)
    {

        // Adding product to db, by getting repo through DI, and deserialize
        // json object into a Product object, and ensure that productid is not provided
        group.MapPost("/upsert", (IUnitOfWork unitOfWork, Actor actor) =>
        {

            try
            {
                bool existInDB = unitOfWork.Actors.ifIDExists(actor.Id);
                if (!existInDB)
                {
                    actor.Id = 0;
                }
            }
            catch (Exception ex)
            {

            }

            try
            {
                if(actor.Id == 0)
                {
                    unitOfWork.Actors.Add(actor);
                    unitOfWork.Actors.SaveChanges();
                    return Results.Ok(new
                    {
                        status = "Successfully Added Movie to List!",
                        createdItem = actor
                    });
                }

                unitOfWork.Movies.Update(actor);
                unitOfWork.Movies.SaveChanges();
                return Results.Ok(new
                {
                    message = "Successfully Updated Movie!",
                    updatedItem = actor
                });
            }
            catch
            {
                return Results.Json(new { message = "Something went wrong, could not add movie to database" }, statusCode: StatusCodes.Status500InternalServerError);
            }
        }).AddEndpointFilter<ValidationFilter<Movie>>();

        // TODO: a method to add or update multiple movies at a time
    }

    public static void ActorDeleteAPI(RouteGroupBuilder group)
    {
        
    }
}