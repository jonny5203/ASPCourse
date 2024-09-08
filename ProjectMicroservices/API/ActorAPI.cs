using ProjectMicroservices.Model;

namespace ProjectMicroservices.API;

static public class ActorAPI
{
    public static RouteGroupBuilder ActorGetAPI(this RouteGroupBuilder group)
    {
        return group;
    }

    public static void ActorPostAPI(this RouteGroupBuilder group, Actor actor)
    {
        group.MapPost("", )
    }

    public static void ActorPutAPI(this RouteGroupBuilder group, Actor actor)
    {
        
    }

    public static void ActorDeleteAPI(this RouteGroupBuilder group, Actor actor)
    {
        
    }

    public static void ActorGetAPI(this RouteGroupBuilder group, Actor actor)
    {
        
    }
}