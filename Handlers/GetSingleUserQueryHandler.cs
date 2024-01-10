using CrudApiAssignment.DTOs;
using CrudApiAssignment.Interfaces;
using CrudApiAssignment.Models;
using CrudApiAssignment.Queries;
using CrudApiAssignment.Utilities;
using MediatR;

namespace CrudApiAssignment.Handlers;

public class GetSingleUserQueryHandler : IRequestHandler<GetSingleUserQuery, ApiResult<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    public GetSingleUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<ApiResult<UserResponse>> Handle(GetSingleUserQuery request, CancellationToken cancellationToken)
    {
        try 
        {
            var user = await _userRepository.GetSingleUser(request.Id);
            if (user == null) 
            {
                return ApiResult<UserResponse>.Failure(ErrorType.ErrUserNotFound, "User was not found");
            }
            return ApiResult<UserResponse>.Success(user);
        }
        catch (Exception ex) 
        {
            return ApiResult<UserResponse>.Failure(ErrorType.ErrUnknown, ex.Message);
        }
    }
}
