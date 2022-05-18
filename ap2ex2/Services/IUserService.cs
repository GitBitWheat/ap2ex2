using ap2ex2.Models;
namespace ap2ex2.Services;

public interface IUserService
{
    public List<User> GetAllUsers(); 
    public User? GetUser(int id);
    public bool Login(string username, string password);
    int AddUser(User user);
    public List<User>? GetContacts(int id);
    public void AddContacts(int id1, int id2);
}