using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SecureWebApi.Tests.TestHelpers.Builders
{
    public class HttpRequestMessageBuilder
    {
        private readonly HttpMethod _method;
        private readonly string _url;
        private AuthenticationHeaderValue? _authorization;

        public HttpRequestMessageBuilder(HttpMethod method, string url)
        {
            _method = method;
            _url = url;
            _authorization = null;
        }

        public static HttpRequestMessageBuilder Create(HttpMethod method, string url) => 
            new  HttpRequestMessageBuilder(method, url);
        
        public HttpRequestMessage Build()
        {
            return new HttpRequestMessage(_method, _url)
            {
                Headers = { Authorization = _authorization}
            };
        }
        
        public HttpRequestMessageBuilder WithBasicAuth(string username, string password)
        {
            var base64String = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(username + ":" + password));
            _authorization = new AuthenticationHeaderValue("Basic", base64String);
            return this;
        }
        
        public HttpRequestMessageBuilder WithJwtAuth(string token)
        {
            _authorization = new AuthenticationHeaderValue("Bearer", token);
            return this;
        }
    }
}