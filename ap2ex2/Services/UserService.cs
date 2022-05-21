using System.Collections.Generic;
using ap2ex2.Models;
namespace ap2ex2.Services;

public class UserService : IUserService
{
    private static List<User> users = new List<User>()
    {
        new User(){Id = 1, Username = "user1", Nickname = "First User", Password = "User1", Pfp = "..."},
        new User(){Id = 2, Username = "user2", Nickname = "Second User", Password = "User2", Pfp = "..."},
        new User(){Id = 3, Username = "user3", Nickname = "Third User", Password = "User3", Pfp = "..."}
    };

    public List<User> GetAllUsers()
    {
        return users;
    }

    public User? GetUser(int? id)
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

    public User? GetUserByUsername(string username)
    {
        return users.Find(user => user.Username == username);
    }

    public bool Login(string username, string password)
    {
        User? user = GetUserByUsername(username);
        if (user != null && user.Password == password)
        {
            return true;
        }

        return false;
    }

    public List<User>? GetContacts(int id)
    {
        User? user = GetUser(id);
        if (null == user)
            return null;
        else
            return user.Contacts;
    }

    public void AddContacts(int? id1, int id2)
    {
        if (id1 == id2)
        {
            return;
        }
        User? user1 = GetUser(id1);
        User? user2 = GetUser(id2);
        if (null == user1 || null == user2)
        {
            return;
        }
        if (user1.Contacts.Contains(user2))
        {
             return;
        }
        user1.Contacts.Add(user2);
        user2.Contacts.Add(user1);
    }
}