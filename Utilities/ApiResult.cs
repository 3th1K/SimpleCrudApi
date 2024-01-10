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

    public static ApiResult<T> Success(T successObject)
    {
        ObjectResult result = new ObjectResult(successObject)
        {
            StatusCode = 200
        };
        return new ApiResult<T>(result, ApiResultStatus.Success);
    }

    public static ApiResult<T> Failure(ErrorType error, string errorMessage = "_", List<string>? errors = null)
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
                    ErrorSolution = "Please provide a valid username",
                    ValidationErrors = errors
                };
                break;
            case ErrorType.ErrUserNotAuthorized:
                createdError = new ErrorResult
                {
                    ErrorType = "User is not authorized",
                    StatusCode = (int)error,
                    ErrorMessage = errorMessage,
                    ErrorSolution = "Please provide correct credentials",
                    ValidationErrors = errors
                };
                break;
            case ErrorType.ErrRequestValidationFailed:
                createdError = new ErrorResult
                {
                    ErrorType = "Request Validation Failed",
                    StatusCode = (int)error,
                    ErrorMessage = errorMessage,
                    ErrorSolution = "Please provide valid inputs before making request",
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
