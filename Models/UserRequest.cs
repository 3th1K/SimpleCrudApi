using CrudApiAssignment.DTOs;
using CrudApiAssignment.Utilities;
using MediatR;

namespace CrudApiAssignment.Models
{
    public class UserRequest : IRequest<ApiResult<User>>
    {
        public string? Username { get; set; } = null;
        public string? Password { get; set; } = null;
        public bool IsAdmin { get; set; } = false;
        public int? Age { get; set; } = null;
        public  string[]? Hobbies { get; set; } = null;
    }
}
