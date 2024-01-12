using CrudApiAssignment.Exceptions;
using CrudApiAssignment.Interfaces;
using CrudApiAssignment.Models;
using CrudApiAssignment.Utilities;
using MediatR;

namespace CrudApiAssignment.Handlers;

public class LoginRequestHandler : IRequestHandler<LoginRequest, ApiResult<string>>
{
    private readonly IIdentityRepository _repository;
    public LoginRequestHandler(IIdentityRepository repository)
    {

        _repository = repository;

    }
    public async Task<ApiResult<string>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var token = await _repository.GetToken(request.Username, request.Password);
            return ApiResult<string>.Success(token);
        }
        //catch (UserNotFoundException ex)
        //{
        //    return ApiResult<string>.Failure(ErrorType.ErrUserNotFound, "User is not a registered user");
        //}
        catch (UserNotAuthorizedException ex)
        {
            return ApiResult<string>.Failure(ErrorType.ErrUserNotAuthorized, "Incorrect password");
        }
        //catch (Exception ex)
        //{
        //    return ApiResult<string>.Failure(ErrorType.ErrUnknown);
        //}
    }
}
