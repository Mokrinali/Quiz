using QuizApp.Core.Models;

namespace QuizApp.Core.Services
{
    public class UserService
    {
        private readonly List<User> _users;

        public UserService(List<User> users)
        {
            _users = users;
        }

        public User Register(string username, string password)
        {
            if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("This username is already taken.");
                return null;
            }

            var newUser = new User { Username = username, Password = password };
            _users.Add(newUser);
            return newUser;
        }

        public User Login(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password)
                   ?? throw new Exception("Invalid username or password.");
        }
    }
}
