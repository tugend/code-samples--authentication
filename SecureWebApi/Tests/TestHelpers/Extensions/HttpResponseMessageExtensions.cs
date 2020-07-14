using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;

namespace SecureWebApi.Tests.TestHelpers.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<HttpResponseMessage> AssertStatusCode(this Task<HttpResponseMessage> pendingResponse, HttpStatusCode expectedCode)
        {
            var response = await pendingResponse;
            
            var statusCode = response.StatusCode;

            var readAsStringAsync = await response.Content.ReadAsStringAsync();
            statusCode.Should().Be(expectedCode, "\n\treceived response: " + readAsStringAsync);
            
            return response;
        }

        public static async Task<HttpResponseMessage> AssertContent(this Task<HttpResponseMessage> pendingResponse, string expectedContent)
        {
            var response = await pendingResponse;
            var serializedContent = await response.Content.ReadAsStringAsync();
            
            serializedContent.Should().BeEquivalentTo(expectedContent);
            
            return response;
        }
        
        public static async Task<HttpResponseMessage> AssertContent<T>(this Task<HttpResponseMessage> pendingResponse, T expectedContent)
        {
            var response = await pendingResponse;
            var serializedContent = await response.Content.ReadAsStringAsync();
            
            var content = JsonConvert.DeserializeObject<T>(serializedContent);
           
            content.Should().BeEquivalentTo(expectedContent);
            
            return response;
        }    
        
        public static async Task<T> Deserialize<T>(this Task<HttpResponseMessage> pendingResponse)
        {
            var response = await pendingResponse;
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}