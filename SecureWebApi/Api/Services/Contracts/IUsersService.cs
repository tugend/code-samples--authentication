using System.Threading.Tasks;
using SecureWebApi.Infrastructure.Authentication;

namespace SecureWebApi.Services.Contracts
{
    public interface IUserService
    {
        Task<bool> TryAuthenticate(string username, string password, out User user);
      
        User GetById(string userId);
    }
}