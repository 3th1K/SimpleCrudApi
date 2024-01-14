using CrudApiAssignment.Utilities;
using MediatR;

namespace CrudApiAssignment.Models;

public class SearchUserRequest : IRequest<ApiResult<SearchUserResponse>>
{
    public string FieldName { get; set; }
    public string FieldValue { get; set; }
    public int PageNumber { get; set; } = 1;
    public int UsersPerPage { get; set; } = 5;
    public bool IsSortAscending { get; set; } = true;
}
