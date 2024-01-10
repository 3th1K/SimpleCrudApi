using CrudApiAssignment.Exceptions;
using CrudApiAssignment.Interfaces;

namespace CrudApiAssignment.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        
        public async Task<string> GetToken(string username, string password)
        {
            if (username != "mehedi") 
            {
                throw new UserNotFoundException();
            }
            if (password != "rahaman") 
            {
                throw new UserNotAuthorizedException();
            }
            return "aojfoajfoadjfassdaddc test token";
        }
    }
}
