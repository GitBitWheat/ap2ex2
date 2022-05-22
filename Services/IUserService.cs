using Domain;
namespace Services;

public interface IUserService
{
    public List<User> GetAllUsers(); 
    public User? GetUser(int? id);
    public User? GetUserByUsername(string username);
    public bool Login(string username, string password);
    int AddUser(User user);
    public List<User>? GetContacts(int id);
    public void AddContacts(int? id1, int id2);
    public bool doesUsernameExist(string username);
    public void addMessage(Message message);
}