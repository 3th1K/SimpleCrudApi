using CrudApiAssignment.Interfaces;
using CrudApiAssignment.Models;
using CrudApiAssignment.Queries;
using CrudApiAssignment.Utilities;
using MediatR;

namespace CrudApiAssignment.Handlers;

public class DeleteUserQueryHandler : IRequestHandler<DeleteUserQuery, ApiResult<User>>
{
    private readonly IUserRepository _userRepository;
    public DeleteUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<ApiResult<User>> Handle(DeleteUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.DeleteUser(request.Id);
        if (user != null)
        {
            return ApiResult<User>.SuccessNoContent(204);
        }
        return ApiResult<User>.Failure(ErrorType.ErrUserNotFound, "User do not exists", "Please provide a valid user id to delete the user");
    }
}
