using ap2ex2.Models;

namespace ap2ex2.Services
{
    public class UsersService : IUsersService
    {
        private List<User> users = new List<User>();

        public void addUser(string username, string password, string nickname, string pfp)
        {
            users.Add(new User() { Userame = username, Password = password, Nickname = nickname, Pfp = pfp });
        }

        public User? getUser(string username)
        {
            return users.Find(user => user.Userame == username);
        }
    }
}
