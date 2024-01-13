using CrudApiAssignment.Models;
using FluentValidation;

namespace CrudApiAssignment.Validations;

public class UserRequestValidator : AbstractValidator<UserRequest>
{
    public UserRequestValidator()
    {

        foreach (var property in typeof(UserRequest).GetProperties())
        {
            RuleFor(x => property.GetValue(x))
                .NotNull().WithMessage($"{property.Name} is missing from the request");
        }
    }
}
