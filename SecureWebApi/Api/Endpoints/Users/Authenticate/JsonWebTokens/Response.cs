using System.ComponentModel.DataAnnotations;

namespace SecureWebApi.Endpoints.Users.Authenticate.JsonWebTokens
{
    public class Response
    {
        [Required]
        public string UserId { get; }
        
        [Required]
        public string UserName { get; }
        
        [Required]
        public string Token { get; }

        public Response(string userId, string userName, string token)
        {
            UserId = userId;
            UserName = userName;
            Token = token;
        }
    }
}