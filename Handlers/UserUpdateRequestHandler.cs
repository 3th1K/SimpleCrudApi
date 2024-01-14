using CrudApiAssignment.Interfaces;
using CrudApiAssignment.Models;
using CrudApiAssignment.Utilities;
using MediatR;
using System.Security.Claims;

namespace CrudApiAssignment.Handlers;

public class UserUpdateRequestHandler : IRequestHandler<UserUpdateRequest, ApiResult<User>>
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserUpdateRequestHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository; 
        _httpContextAccessor = httpContextAccessor;
    }
    private (string? UserId, string? UserRole) GetAuthenticatedUser()
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirst("userId")?.Value;
        var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        return (userId, userRole);
    }
    public async Task<ApiResult<User>> Handle(UserUpdateRequest request, CancellationToken cancellationToken)
    {
        var (authenticatedUserId, authenticatedUserRole) = GetAuthenticatedUser();
        if (authenticatedUserRole != "Admin" && authenticatedUserId != request.Id.ToString())
        {
            return ApiResult<User>.Failure(ErrorType.ErrUserForbidden, "User is not allowed to access this content");
        }
        var updatedUser = await _userRepository.UpdateUser(request);
        if (updatedUser == null)
        {
            return ApiResult<User>.Failure(ErrorType.ErrUserNotFound, "Cannot Update The User, User is not a valid user");
        }

        return ApiResult<User>.Success(updatedUser);
    }
}

public class SearchUserRequestHandler : IRequestHandler<SearchUserRequest, ApiResult<SearchUserResponse>>
{
    private readonly IUserRepository _userRepository;
    public SearchUserRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<ApiResult<SearchUserResponse>> Handle(SearchUserRequest request, CancellationToken cancellationToken)
    {
        var data = await _userRepository.SearchUser(request);
        if (data.TotalCount == 0) 
        {
            return ApiResult<SearchUserResponse>.Failure(ErrorType.ErrEmptySearchResult, "Empty search result", "Try searching with different field or value");
        }
        return ApiResult<SearchUserResponse>.Success(data);
    }
}
