using AutoMapper;
using CrudApiAssignment.Interfaces;
using CrudApiAssignment.Models;
using CrudApiAssignment.Utilities;
using MediatR;

namespace CrudApiAssignment.Handlers;

public class UserRequestHandler : IRequestHandler<UserRequest, ApiResult<User>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    public UserRequestHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<ApiResult<User>> Handle(UserRequest request, CancellationToken cancellationToken)
    {
        var addedUser = await _userRepository.CreateUser(request);
        return ApiResult<User>.Success(addedUser, 201);
    }
}
