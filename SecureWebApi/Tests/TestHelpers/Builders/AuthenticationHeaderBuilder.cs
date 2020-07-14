using System.Net.Http.Headers;
using static System.Text.Encoding;
using Convert = System.Convert;

namespace SecureWebApi.Tests.TestHelpers.Builders
{
    public static class AuthenticationHeaderBuilder
    {
        public static AuthenticationHeaderValue BuildBasic(string username, string password)
        {
            var base64String = Convert.ToBase64String(ASCII.GetBytes(username + ":" + password));
            return new AuthenticationHeaderValue("Basic", base64String);
        }
    }
}