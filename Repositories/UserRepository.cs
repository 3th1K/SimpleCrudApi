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
            if (id == "187740c5-41b2-45f8-9f01-8415632f39a5") 
            {
                return new UserResponse() 
                {
                    Id = "abc",
                    Username = "user1",
                    Age = 1,
                    Hobbies = new string[] { "game", "cricket", "coding" }
                };

            }
            return null;
        }
    }
}
