using ProjectMicroservices.Model.BaseModel;
using FluentValidation;

namespace ProjectMicroservices.Services.Validation;

public class ValidateIdFromObj : AbstractValidator<BaseEntity>
{
    public ValidateIdFromObj()
    {
        RuleFor(x => x.Id)
            .Empty()
            .Unless(x => x.Id == 0)
            .WithMessage("The ID must be either empty or 0.") // Custom message for Empty rule
            .Equal(0)
            .When(x => x.Id != null)
            .WithMessage("If an ID is provided, it must be 0."); // Custom message for Equal(0) rule
    }
}