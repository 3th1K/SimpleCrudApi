using CrudApiAssignment.Models;
using CrudApiAssignment.Utilities;
using MediatR;

namespace CrudApiAssignment.Queries;

public class DeleteUserQuery : IRequest<ApiResult<User>>
{
    public readonly string Id;
    public DeleteUserQuery(string id)
    {
        Id = id;
    }
}
