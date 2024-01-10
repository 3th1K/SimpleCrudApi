using CrudApiAssignment.DTOs;
using CrudApiAssignment.Interfaces;
using CrudApiAssignment.Models;

namespace CrudApiAssignment.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<List<User>> GetAllUsers()
        {
            return new List<User>();
        }

        public async Task<UserResponse> GetSingleUser(string id)
        {
            throw new NotImplementedException();
        }
    }
}
