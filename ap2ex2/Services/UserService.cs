using ap2ex2.Models;
namespace ap2ex2.Services;

public class UserService : IUserService
{
    private static List<User> users = new List<User>();

    public List<User> GetAllUsers()
    {
        return users;
    }

    public User? GetUser(int id)
    {
        return users.Find(user => user.Id == id);
    }

    public int AddUser(User user)
    {
        int nextId;
        if (users.Count == 0)
            nextId = 1;
        else
            nextId = users.Max(user => user.Id) + 1;
        user.Id = nextId;
        users.Add(user);
        return nextId;
    }

    public int AddUser(string username, string nickname, string password, string image)
    {
        int nextId;
        if (users.Count == 0)
            nextId = 1;
        else
            nextId = users.Max(user => user.Id) + 1;
        users.Add(new User() {Id = nextId, Username = username, Nickname = nickname, Password = password, Pfp = image });
        return nextId;
    }

    public void EditUser(int id, string username, string nickname, string password, string image)
    {
        User? user = GetUser(id);
        user.Username = username;
        user.Nickname = nickname;
        user.Password = password;
        user.Pfp = image;
    }

    public bool Login(string username, string password)
    {
        User user = GetUserByUsername(username);
        if (user != null && user.Password == password)
        {
            return true;
        }
        return false;
    }
}