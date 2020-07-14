using System.ComponentModel.DataAnnotations;

namespace SecureWebApi.Endpoints.Users.Authenticate.Basic
{
    public class Request
    {
        [Required] 
        public string Username  { get; }
        
        [Required] 
        public string Password { get; }

        public Request(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}