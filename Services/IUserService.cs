using Domain;
namespace Services;

public interface IUserService
{
    public List<User> GetAllUsers(); 
    public User? GetUser(string id);
    public bool Login(string username, string password);
    void AddUser(User user);
    public List<User>? GetContacts(string id);
    public void AddContacts(string id1, string id2);
    public bool doesUserExist(string id);
    public void addMessage(Message message);
    public bool isContactOfUser(string userId, string contactId);
    public List<Message>? getMessagesBetweenTwoUsers(string id1, string id2);
    public void SendMessage(string message, string sentFromId, string sendToId);
}