namespace CrudApiAssignment.DTOs
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public string[] Hobbies { get; set; } = [];
    }
}
