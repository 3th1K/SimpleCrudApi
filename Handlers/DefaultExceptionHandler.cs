using CrudApiAssignment.Exceptions;
using CrudApiAssignment.Models;
using CrudApiAssignment.Utilities;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CrudApiAssignment.Handlers;

public class DefaultExceptionHandler : IExceptionHandler
{
    public DefaultExceptionHandler()
    {
        
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ApiResult<ErrorResult> errResult;

        switch (exception) 
        {
            case UserNotFoundException:
                errResult = ApiResult<ErrorResult>.Failure(ErrorType.ErrUserNotFound, exception.Message);
                break;
            case UserNotAuthorizedException:
                errResult = ApiResult<ErrorResult>.Failure(ErrorType.ErrUserNotAuthorized, exception.Message);
                break;
            case ValidationException:
                ValidationException ex = (ValidationException)exception;
                errResult = ApiResult<ErrorResult>.Failure(ErrorType.ErrRequestValidationFailed, "Validation Falied", null, ex.Errors.Select(error => error.ErrorMessage).ToList());
                break;
            default:
                errResult = ApiResult<ErrorResult>.Failure(ErrorType.ErrUnknown, exception.Message);
                break;
        }

        httpContext.Response.StatusCode = (int)errResult.Result.StatusCode!;
        await httpContext.Response.WriteAsJsonAsync(errResult.Result.Value);
        return true;
    }
}
