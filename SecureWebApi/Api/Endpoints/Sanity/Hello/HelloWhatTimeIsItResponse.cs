using System;

namespace SecureWebApi.Endpoints.Sanity.Hello
{
    public class HelloWhatTimeIsItResponse
    {
        public string Value { get; }
        public DateTimeOffset? Time { get; }

        public HelloWhatTimeIsItResponse(string value, DateTimeOffset time)
        {
            Time = time;
            Value = value;
        }
    }
}