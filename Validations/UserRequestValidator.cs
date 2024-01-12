using CrudApiAssignment.Models;
using FluentValidation;

namespace CrudApiAssignment.Validations;

public class UserRequestValidator : AbstractValidator<UserRequest>
{
    public UserRequestValidator()
    {
        //RuleFor(x=> x.Username).Empty()!.WithMessage("Please provide username");
        //RuleFor(x => x.Password).Empty().WithMessage("Please provide password");
        //RuleFor(x => x.Age).Empty().WithMessage("Please provide age");
        //RuleFor(x => x.Hobbies).Empty().WithMessage("Please provide hobbies, if no hobbies privide empty list");

        foreach (var property in typeof(UserRequest).GetProperties())
        {
            RuleFor(x => property.GetValue(x))
                .NotNull().WithMessage($"{property.Name} is missing from the request");
        }
    }
}
