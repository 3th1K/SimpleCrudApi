namespace CrudApiAssignment.Exceptions;

public class UserNotAuthorizedException : Exception
{
    public UserNotAuthorizedException()
    {
        
    }
    public UserNotAuthorizedException(string message) : base(message) { }
}
