namespace Web.Services
{
    public class User
    {
        public User(string username, string email)
        {
            Username = username;
            Email = email;
        }

        public string Username { get; }
        public string Email { get; }
    }
}
