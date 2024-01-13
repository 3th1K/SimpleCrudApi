using CrudApiAssignment.Queries;
using FluentValidation;

namespace CrudApiAssignment.Validations;

public class DeleteUserQueryValidator : AbstractValidator<DeleteUserQuery>
{
    public DeleteUserQueryValidator()
    {
        RuleFor(x => x.Id).Must(IsValidUuid).WithMessage("Given user id is not an valid UUID");
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
