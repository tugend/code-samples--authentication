using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using static SecureWebApi.Infrastructure.Swagger.ApiDefinitions;

namespace SecureWebApi.Endpoints.Sanity.Hello
{
    [ApiExplorerSettings(GroupName = PublicApiGroupName)]
    [ApiController]
    [Route("sanity/hello")]
    public class HelloController : ControllerBase
    {
        private readonly ISystemClock _clock;

        public HelloController(ISystemClock clock) => _clock = clock;

        [HttpGet()]
        public HelloResponse Hello() => new HelloResponse("Hello world!");
        
        [HttpGet("what-time-is-it")]
        public HelloWhatTimeIsItResponse WhatTimeIsIt() => new HelloWhatTimeIsItResponse("Hello, here is the time!", _clock.UtcNow);
    }
}