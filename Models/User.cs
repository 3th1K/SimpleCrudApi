namespace CrudApiAssignment.Models;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; } = false;
    public int Age { get; set; }
    public string[] Hobbies { get; set; } = [];
}
