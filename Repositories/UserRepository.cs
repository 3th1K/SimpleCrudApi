using AutoMapper;
using CrudApiAssignment.DTOs;
using CrudApiAssignment.Interfaces;
using CrudApiAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudApiAssignment.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<User> CreateUser(UserRequest userRequest)
        {
            var user = _mapper.Map<User>(userRequest);
            user.Id = Guid.NewGuid().ToString();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var addedUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == user.Id);
            return addedUser;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<UserResponse> GetSingleUser(string id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            var userResponse = _mapper.Map<UserResponse>(user);
            return userResponse;
        }

        public async Task<User> UpdateUser(UserUpdateRequest user)
        {
            var userInDb = await _context.Users.SingleOrDefaultAsync(u => u.Id == user.Id);
            if (userInDb != null)
            {
                userInDb.Username = user.Username ?? userInDb.Username;
                userInDb.Password = user.Password ?? userInDb.Password;
                userInDb.Age = user.Age ?? userInDb.Age;
                userInDb.Hobbies = user.Hobbies ?? userInDb.Hobbies;
                await _context.SaveChangesAsync();
                var updatedUserInDb = await _context.Users.SingleOrDefaultAsync(u => u.Id == user.Id);
                return updatedUserInDb!;
            }
            return userInDb;
        }

        public async Task<User?> DeleteUser(string id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }
    }
}
