namespace CrudApiAssignment.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException()
    {
        
    }
    public UserNotFoundException(string message, string? solution=null) : base(message) { }
    
    
}
