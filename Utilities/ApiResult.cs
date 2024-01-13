using CrudApiAssignment.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrudApiAssignment.Utilities;

public enum ApiResultStatus
{
    Success, Error
}
public class ApiResult<T>
{

    public ObjectResult Result { get; private set; }

    public ApiResultStatus ResultStatus { get; private set; }

    public ApiResult()
    {
    }

    private ApiResult(ObjectResult result, ApiResultStatus resultStatus)
    {
        ResultStatus = resultStatus;
        Result = result;
    }

    public static ApiResult<T> Success(T successObject, int? statusCode=null)
    {
        ObjectResult result = new ObjectResult(successObject)
        {
            StatusCode = statusCode??200
        };
        return new ApiResult<T>(result, ApiResultStatus.Success);
    }
    public static ApiResult<T> SuccessNoContent(int? statusCode = null)
    {
        ObjectResult result = new ObjectResult(null)
        {
            StatusCode = statusCode ?? 200
        };
        return new ApiResult<T>(result, ApiResultStatus.Success);
    }

    public static ApiResult<T> Failure(ErrorType error, string errorMessage = "_", string? solution=null, List<string>? errors = null)
    {
        ErrorResult createdError = new();
        switch (error) 
        {
            case ErrorType.ErrUserNotFound:
                createdError = new ErrorResult
                {
                    ErrorType = "User Not Found",
                    StatusCode = (int)error,
                    ErrorMessage = errorMessage,
                    ErrorSolution = solution??"Please provide a valid username",
                    ValidationErrors = errors
                };
                break;
            case ErrorType.ErrUserNotAuthorized:
                createdError = new ErrorResult
                {
                    ErrorType = "User is not authorized",
                    StatusCode = (int)error,
                    ErrorMessage = errorMessage,
                    ErrorSolution = solution ?? "Please provide correct credentials",
                    ValidationErrors = errors
                };
                break;
            case ErrorType.ErrRequestValidationFailed:
                createdError = new ErrorResult
                {
                    ErrorType = "Request Validation Failed",
                    StatusCode = (int)error,
                    ErrorMessage = errorMessage,
                    ErrorSolution = solution ?? "Please provide valid inputs before making request",
                    ValidationErrors = errors
                };
                break;
            case ErrorType.ErrUserForbidden:
                createdError = new ErrorResult
                {
                    ErrorType = "Action Forbidden",
                    StatusCode = (int)error,
                    ErrorMessage = errorMessage,
                    ErrorSolution = solution ?? "This action is forbidden",
                    ValidationErrors = errors
                };
                break;
            default:
                createdError = new ErrorResult
                {
                    ErrorType = "Internal Server Error",
                    StatusCode = 500,
                    ErrorMessage = errorMessage??"Server crashed unexpectedly",
                    ErrorSolution = solution ?? "Please check internal code",
                    ValidationErrors = errors
                };
                break;
        }
        
        ObjectResult result = new ObjectResult(createdError)
        {
            StatusCode = createdError.StatusCode
        };
        return new ApiResult<T>(result, ApiResultStatus.Error);
    }
}
