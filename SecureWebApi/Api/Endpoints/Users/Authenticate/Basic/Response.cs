using System.ComponentModel.DataAnnotations;

namespace SecureWebApi.Endpoints.Users.Authenticate.Basic
{
    public class Response
    {
        [Required] 
        public string UserId { get; }
        
        [Required] 
        public string Username { get; }

        public Response(string userId, string username)
        {
            UserId = userId;
            Username = username;
        }
    }
}