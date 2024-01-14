using CrudApiAssignment.Models;
using FluentValidation;

namespace CrudApiAssignment.Validations;

public class SearchUserRequestValidator : AbstractValidator<SearchUserRequest>
{
    public SearchUserRequestValidator()
    {
        RuleFor(x => x.FieldName).NotEmpty().WithMessage("Field names required");
        RuleFor(x => x.FieldValue).NotEmpty().WithMessage("Field value is required");
    }
}
