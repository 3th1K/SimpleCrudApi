using CrudApiAssignment.Interfaces;
using CrudApiAssignment.Models;
using CrudApiAssignment.Queries;
using CrudApiAssignment.Utilities;
using MediatR;

namespace CrudApiAssignment.Handlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ApiResult<List<User>>>
    {
        private readonly IUserRepository _userRepository;
        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ApiResult<List<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUsers();
            return ApiResult<List<User>>.Success(users);
        }
    }
}
