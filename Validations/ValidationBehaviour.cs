using CrudApiAssignment.Models;
using CrudApiAssignment.Utilities;
using FluentValidation;
using MediatR;

namespace CrudApiAssignment.Validations;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IValidator<TRequest> _validators;
    public ValidationBehavior(IValidator<TRequest> validators)
    {
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationResult = await _validators.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors;
            throw new ValidationException(validationErrors);
        }
        return await next();
    }
}
