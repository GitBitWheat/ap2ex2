using Domain;
namespace Services;

public interface IUserService
{
    public List<User> GetAllUsers(); 

    public bool GetUser(string userId, out User user);

    void AddUser(User user);

    public bool Login(string username, string password);

    public bool DoesUserExist(string userId);

    public bool GetContacts(string userId, out List<Contact> contactList);

    public bool GetContactOfId(string userId, string contactId, out Contact requestedContact);

    public bool UpdateContactOfId(string userId, string contactId, string name, string server);

    public bool AddContact(string userId, Contact newContact);

    public bool RemoveContact(string userId, string contactId);

    public bool IsContactOfUser(string userId, string contactId);

    public bool GetMessagesBetweenUserAndContact(string userId, string contactId, out List<Message> msgList);

    public bool GetMessageOfIdBetweenUserAndContact(string userId, string contactId, int messageId, out Message requestedMessage);

    public bool UpdateMessageOfIdBetweenUserAndContact(string userId, string contactId, int messageId, string content);

    public bool SendMessage(string messageContent, string sentFromUserId, string sendToContactId);

    public bool ReceiveMessage(string messageContent, string sentFromContactId, string sendToUserId);

    public bool RemoveMessage(string userId, string contactId, int messageId);
}