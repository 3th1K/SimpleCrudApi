using CrudApiAssignment.Utilities;
using MediatR;

namespace CrudApiAssignment.Models
{
    public class UserUpdateRequest : IRequest<ApiResult<User>>
    {
        public string Id { get; set; }
        public string? Username { get; set; } = null;
        public string? Password { get; set; } = null;
        public int? Age { get; set; } = null;
        public string[]? Hobbies { get; set; } = null;
    }
}
