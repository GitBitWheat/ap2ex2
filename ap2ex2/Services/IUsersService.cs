using ap2ex2.Models;

namespace ap2ex2.Services
{
    public interface IUsersService
    {
        public void addUser(string username, string password, string nickname, string pfp);

        public User? getUser(string username);
    }
}
