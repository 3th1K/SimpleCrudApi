using CrudApiAssignment.Exceptions;
using CrudApiAssignment.Interfaces;
using CrudApiAssignment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CrudApiAssignment.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public IdentityRepository(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<string> GetToken(string username, string password)
        {
            User user = await ValidateUser(username, password);
            var token = GenerateJwtToken(user);
            return token;
        }


        private async Task<User> ValidateUser(string username, string password)
        {
            //try
            //{
            //    User? user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            //    var x = "aaa";
            //    if (user == null)
            //    {
            //        throw new UserNotFoundException($"{username} is not an regestered user");
            //    }
            //    if (user.Password != password)
            //    {
            //        throw new UserNotAuthorizedException("Incorrect Password");
            //    }
            //    return user;
            //}
            //catch (Exception ex) 
            //{
            //    throw;
            //}
            User? user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new UserNotFoundException($"{username} is not an regestered user");
            }
            if (user.Password != password)
            {
                throw new UserNotAuthorizedException("Incorrect Password");
            }
            return user;

        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var role = user.IsAdmin ? "Admin" : "User";
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Username));
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim(ClaimTypes.Name, $"{user.Username}"));
            claims.Add(new Claim("userId", user.Id.ToString()));

            var identity = new ClaimsIdentity(claims);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse(jwtSettings["TokenLifeHours"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
