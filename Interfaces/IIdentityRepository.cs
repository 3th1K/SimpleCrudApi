namespace CrudApiAssignment.Interfaces
{
    public interface IIdentityRepository
    {
        public Task<string> GetToken(string username, string password);
    }
}
