namespace SecureWebApi.Infrastructure.Authentication
{
    public class User
    {
        public readonly string Id;
        public readonly string Username;
        public readonly string Password;
        public readonly int ClearanceLevel;
        
        public static readonly User None = new User("", "", "", -1);

        public User(string id, string username, string password, int clearanceLevel)
        {
            Id = id;
            Username = username;
            Password = password;
            ClearanceLevel = clearanceLevel;
        }
    }
}