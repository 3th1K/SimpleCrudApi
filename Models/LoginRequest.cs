using CrudApiAssignment.Utilities;
using MediatR;

namespace CrudApiAssignment.Models
{
    public record LoginRequest : IRequest<ApiResult<string>>
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
