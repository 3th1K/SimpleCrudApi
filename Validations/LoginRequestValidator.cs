using CrudApiAssignment.Models;
using FluentValidation;

namespace CrudApiAssignment.Validations
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .MinimumLength(5).WithMessage("Username Must be minimum length of 5")
                .Must(BeAValidUsername).WithMessage("Username cannot be an integer");
        }
        private bool BeAValidUsername(string username)
        {

            if (int.TryParse(username, out _))
            {
                return false;
            }

            return true;
        }
    }
}
