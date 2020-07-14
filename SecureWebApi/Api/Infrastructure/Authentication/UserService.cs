using System.Linq;
using System.Threading.Tasks;
using SecureWebApi.Services.Contracts;

namespace SecureWebApi.Infrastructure.Authentication
{
    public class AuthenticatedUserService : IUserService
    {
        private readonly User[] _users;

        public AuthenticatedUserService(params User[] users) => _users = users;

        public Task<bool> TryAuthenticate(string username, string password, out User user)
        {
            // NOTE: Never store passwords in clear text, use salted and hashed storage or delegate to a cloud provider
            var match = _users.SingleOrDefault(x => 
                x.Username.Equals(username) && 
                x.Password.Equals(password));
           
            if (match != null)
            {
                user = match;
                return Task.FromResult(true);
            }

            user = User.None;
            return Task.FromResult(false);
        }

        public User GetById(string userId)
        {
            return _users.Single(x => x.Id == userId);
        }
    }
}