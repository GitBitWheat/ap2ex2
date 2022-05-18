using ap2ex2.Models;
namespace ap2ex2.Services;

public interface IUserService
{
    public List<User> GetAllUsers(); 
    public User GetUser(int id);
    public void Register(string username, string nickname, string password, string image);
    public void EditUser(int id, string username, string nickname, string password, string image);
    public bool Login(string username, string password);
}