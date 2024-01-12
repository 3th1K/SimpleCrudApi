using CrudApiAssignment.DTOs;
using CrudApiAssignment.Models;

namespace CrudApiAssignment.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<UserResponse> GetSingleUser(string id);

        public Task<User> CreateUser(UserRequest userRequest);
    }
}
