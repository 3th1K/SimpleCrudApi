using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrudApiAssignment.Models
{
    public enum ErrorType
    {
        ErrUserNotFound = 404,
        ErrUserNotAuthorized = 401,
        ErrRequestValidationFailed = 400,
        ErrUnknown = 500
    }
    public class ErrorResult
    {
        public string ErrorType { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 500;
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorSolution { get; set; } = string.Empty;

        public List<string>? ValidationErrors { get; set; } = new();
    }
}
