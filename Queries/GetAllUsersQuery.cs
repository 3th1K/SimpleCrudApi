using CrudApiAssignment.Models;
using CrudApiAssignment.Utilities;
using MediatR;

namespace CrudApiAssignment.Queries
{
    public class GetAllUsersQuery : IRequest<ApiResult<List<User>>>
    {
    }
}
