using Domain;
namespace Services;

public interface IUserService
{
    public List<User> GetAllUsers(); 
    public bool GetUser(string id, out User user);
    public bool Login(string username, string password);
    void AddUser(User user);
    public bool GetContacts(string id, out List<User> contactList);
    public bool AddContacts(string id1, string id2);
    public bool RemoveContacts(string id1, string id2);
    public bool DoesUserExist(string id);
    public bool IsContactOfUser(string userId, string contactId);
    public bool GetMessagesBetweenTwoUsers(string id1, string id2, out List<Message> msgList);
    public bool GetMessageOfIdBetweenTwoUsers(string userId1, string userId2, int messageId, out Message requestedMessage);
    public bool SendMessage(string message, string sentFromId, string sendToId);
    public bool RemoveMessage(string userId1, string userId2, int messageId);
}