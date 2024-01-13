using CrudApiAssignment.Models;
using FluentValidation;

namespace CrudApiAssignment.Validations;

public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
{
    public UserUpdateRequestValidator()
    {
        RuleFor(x => x.Id).Must(IsValidUuid).WithMessage("Given user id is not an valid UUID");
        RuleFor(x => x.Username).Length(5, 50).WithMessage("Username should be of length 5 - 50");
        RuleFor(x => x.Password).MinimumLength(8).WithMessage("Password Should be minimum length of 8 characters");
    }
    private bool IsValidUuid(string id)
    {
        if (Guid.TryParse(id, out _))
        {
            return true;
        }
        return false;
    }
}
