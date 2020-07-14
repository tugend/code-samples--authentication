using System.ComponentModel.DataAnnotations;

namespace SecureWebApi.Endpoints.Sanity.Hello
{
    public class HelloResponse
    {
        [Required]
        public string Value { get; }

        public HelloResponse(string value) => Value = value;
    }
}