using CrudApiAssignment.DTOs;
using CrudApiAssignment.Utilities;
using MediatR;

namespace CrudApiAssignment.Queries;

public class GetSingleUserQuery : IRequest<ApiResult<UserResponse>>
{
    public readonly string Id;
    public GetSingleUserQuery(string userId)
    {
        Id = userId;
    }
}
