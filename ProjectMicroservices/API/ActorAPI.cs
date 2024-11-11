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
                return Results.Json(new { message = "No entry exists on actor ID provided" }, statusCode: StatusCodes.Status404NotFound);
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
                return Results.Json(new { message = "No entry exists in actor table" }, statusCode: StatusCodes.Status404NotFound);
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
                    unitOfWork.SaveChanges();
                    return Results.Ok(new
                    {
                        status = "Successfully Added Actor to List!",
                        createdItem = actor
                    });
                }

                unitOfWork.Movies.Update(actor);
                unitOfWork.SaveChanges();
                return Results.Ok(new
                {
                    message = "Successfully Updated Actor!",
                    updatedItem = actor
                });
            }
            catch
            {
                return Results.Json(new { message = "Something went wrong, could not add/update actor to database" }, statusCode: StatusCodes.Status500InternalServerError);
            }
        }).AddEndpointFilter<ValidationFilter<Movie>>();

        // TODO: a method to add or update multiple movies at a time
    }

    public static void ActorDeleteAPI(RouteGroupBuilder group)
    {
        group.MapDelete("", (IUnitOfWork unitOfWork, int id) =>
        {
            try
            {
                var deleted = unitOfWork.Actors.Delete(id);
                if (deleted == null) return Results.NotFound();

                unitOfWork.SaveChanges();

                return Results.Ok(new
                {
                    message = "Successfully Added Actor to List!",
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