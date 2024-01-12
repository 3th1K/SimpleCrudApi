using CrudApiAssignment.Exceptions;
using CrudApiAssignment.Models;
using CrudApiAssignment.Utilities;
using Microsoft.AspNetCore.Diagnostics;
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
        if (exception is UserNotFoundException) 
        {
            await httpContext.Response.WriteAsJsonAsync(ApiResult<string>.Failure(ErrorType.ErrUserNotFound, exception.Message).Result.Value);
            return true;
        }
        return true;
    }
}
