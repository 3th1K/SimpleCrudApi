using CrudApiAssignment.DTOs;
using CrudApiAssignment.Models;

namespace CrudApiAssignment.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsers();
        public Task<UserResponse> GetSingleUser(string id);
        public Task<User> CreateUser(UserRequest userRequest);
        public Task<User> UpdateUser(UserUpdateRequest user);
        public Task<User?> DeleteUser(string id);
    }
}
