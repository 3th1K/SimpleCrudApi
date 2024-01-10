using CrudApiAssignment.Exceptions;
using CrudApiAssignment.Interfaces;
using CrudApiAssignment.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CrudApiAssignment.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly IConfiguration _configuration;
        public IdentityRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> GetToken(string username, string password)
        {
            User user = await ValidateUser(username, password);
            var token = GenerateJwtToken(user);
            return token;
        }


        private async Task<User> ValidateUser(string username, string password)
        {
            if (username != "mehedi")
            {
                throw new UserNotFoundException();
            }
            if (password != "rahaman")
            {
                throw new UserNotAuthorizedException();
            }
            return new User() {
                Id = "someid",
                Username = username,
                Password = password,
                Age = 10,
                Hobbies = [],
                IsAdmin = true
            };
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
