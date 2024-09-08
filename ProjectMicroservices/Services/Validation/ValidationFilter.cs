using FluentValidation;

namespace ProjectMicroservices.Services.Validation;


public class ValidationFilter<BaseEntity> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext ctx, EndpointFilterDelegate next)
    {
        var validator = ctx.HttpContext.RequestServices.GetService<IValidator<BaseEntity>>();
        
        if (validator is not null)
        {
            var entity = ctx.Arguments
                .OfType<BaseEntity>()
                .FirstOrDefault(a => a?.GetType() == typeof(BaseEntity));
            if (entity is not null)
            {
                var validation = await validator.ValidateAsync(entity);
                if (validation.IsValid)
                {
                    return await next(ctx);
                }
                return Results.ValidationProblem(validation.ToDictionary());
            }
            else
            {
                return Results.Problem("Could not find type to validate");
            }
        }
        return await next(ctx);
    }
}