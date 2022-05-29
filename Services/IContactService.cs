using Domain;
namespace Services;

public interface IContactService
{
    public List<Contact> GetContacts(string id);

    public bool AddContact(string userId, Contact newContact);

    public bool RemoveContact(string userId, string contactId);

    public bool IsContactOfUser(string userId, string contactId);

    public bool GetMessagesBetweenContacts(string contactId1, string contactId2, out List<Message> msgList);

    public bool GetMessageOfIdBetweenContacts(string contactId1, string contactId2, int messageId, out Message requestedMessage);

    public bool SendMessage(string messageContent, string sentFromId, string sendToId);

    public bool RemoveMessage(string contactId1, string contactId2, int messageId);
}