using System;
using SecureWebApi.Infrastructure.Authentication;

namespace SecureWebApi.Tests.Endpoints.Users
{
    public class TestUserFactory
    {
        private static readonly Random Random = new Random();
        
        public static User CreateUser(int clearanceLevel)
        {
            var id = Guid.NewGuid().ToString();
            var name = $"{Pick("bob", "jack", "smith")}-{Guid.NewGuid()}";
            const string password = "password";
            
            return new User(id, name, password, clearanceLevel);
        }
        
        public static User CreateUser() => CreateUser(Random.Next(7) + 1);

        private static T Pick<T>(params T[] values) => values[Random.Next(values.Length)];
    }
}