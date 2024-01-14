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

        public async Task<SearchUserResponse> SearchUser(SearchUserRequest searchUserRequest)
        {
            var allUsers = await this.GetAllUsers();

            if (!string.IsNullOrEmpty(searchUserRequest.FieldName) && !string.IsNullOrEmpty(searchUserRequest.FieldValue))
            {
                allUsers = allUsers.Where(user => DoesUserMatchFilter(user, searchUserRequest.FieldName, searchUserRequest.FieldValue)).ToList();
            }
            if (searchUserRequest.IsSortAscending)
            {
                allUsers.Sort((user1, user2) => user1.Username.CompareTo(user2.Username));
            }
            else 
            {
                allUsers.Sort((user1, user2) => user2.Username.CompareTo(user1.Username));
            }
            var paginatedUsers = this.Paginate(allUsers, searchUserRequest.PageNumber,searchUserRequest.UsersPerPage);
            return new SearchUserResponse
            {
                Users = paginatedUsers,
                TotalCount = paginatedUsers.Count(),
                CurrentPage = searchUserRequest.PageNumber,
                TotalPages = (int)Math.Ceiling((decimal)allUsers.Count() / (decimal)searchUserRequest.UsersPerPage),
                SortingOrder = searchUserRequest.IsSortAscending ? "Ascending" : "Descending"
            };
        }

        private bool DoesUserMatchFilter(User user, string fieldName, string fieldValue)
        {
            switch (fieldName.ToLower())
            {
                case "username":
                    return user.Username.Contains(fieldValue, StringComparison.OrdinalIgnoreCase);
                case "isadmin":
                    return user.IsAdmin.Equals(bool.Parse(fieldValue));
                case "age":
                    return user.Age.Equals(int.Parse(fieldValue));
                case "hobbies":
                    var hobbies = fieldValue.Split(',');
                    return hobbies.Any(userHobby => user.Hobbies.Contains(userHobby, StringComparer.OrdinalIgnoreCase));
                default:
                    return false;
            }
        }

        private List<User> Paginate(List<User> totalUsers, int pageNumber,int usersPerPage) 
        {
            var paginatedUsers = new List<User>();
            int start = (pageNumber*usersPerPage)-(usersPerPage-1) - 1;
            //int end = (pageNumber*usersPerPage);
            paginatedUsers = totalUsers.Skip(start).Take(usersPerPage).ToList();
            //for (int i = 0; i < totalUsers.Count; i += usersPerPage)
            //{
            //    var page = totalUsers.Skip(i).Take(usersPerPage).ToList();
            //    paginatedUsers.Add(page);
            //}

            return paginatedUsers;
        }
    }
}
