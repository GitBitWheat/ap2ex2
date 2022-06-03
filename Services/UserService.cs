using System.Collections.Generic;
using Domain;
namespace Services;

public class UserService : IUserService
{
    private static List<User> users = new List<User>();
    private static int msgCount = 0;
    
    public UserService()
    {
    }

    public List<User> GetAllUsers()
    {
        return users;
    }

    public bool GetUser(string userId, out User user)
    {
        User? foundUser = users.Find(user => user.Id.Equals(userId));
        if (foundUser != null)
        {
            user = foundUser;
            return true;
        }
        else
        {
            user = new User();
            return false;
        }
    }

    public void AddUser(User user)
    {
        users.Add(user);
    }

    public bool Login(string username, string password)
    {
        User user;
        if (!GetUser(username, out user))
            return false;

        return user.Password == password;
    }

    public bool DoesUserExist(string userId)
    {
        return users.Exists(u => u.Id.Equals(userId));
    }

    public bool GetContacts(string userId, out List<Contact> contactList)
    {
        User user;
        if (!GetUser(userId, out user))
        {
            contactList = new List<Contact>();
            return false;
        }
        else
        {
            contactList = user.Contacts;
            return true;
        }
    }

    public bool GetContactOfId(string userId, string contactId, out Contact requestedContact)
    {
        List<Contact> contactList;
        if (!GetContacts(userId, out contactList))
        {
            requestedContact = new Contact();
            return false;
        }

        Contact? foundContact = contactList.Find(c => c.Id.Equals(contactId));
        if (null != foundContact)
        {
            requestedContact = foundContact;
            return true;
        }
        else
        {
            requestedContact = new Contact();
            return false;
        }
    }

    public bool UpdateContactOfId(string userId, string contactId, string name, string server)
    {
        Contact contact;
        if (!GetContactOfId(userId, contactId, out contact))
            return false;

        contact.Name = name;
        contact.Server = server;
        return true;
    }

    public bool AddContact(string userId, Contact newContact)
    {
        List<Contact> contactList;
        if (!GetContacts(userId, out contactList))
            return false;

        if (contactList.Exists(c => c.Id.Equals(newContact.Id)))
            return false;
        else
        {
            contactList.Add(newContact);
            return true;
        }
    }

    public bool RemoveContact(string userId, string contactId)
    {
        List<Contact> contactList;
        if (!GetContacts(userId, out contactList))
            return false;

        return contactList.RemoveAll(c => c.Id.Equals(contactId)) > 0;
    }

    public bool IsContactOfUser(string userId, string contactId)
    {
        User user;
        if (GetUser(userId, out user))
            return user.Contacts.Exists(c => c.Id.Equals(contactId));
        else
            return false;
    }



    public bool GetMessagesBetweenUserAndContact(string userId, string contactId, out List<Message> msgList)
    {
        Contact contact;
        if (!GetContactOfId(userId, contactId, out contact))
        {
            msgList = new List<Message>();
            return false;
        }
        else
        {
            msgList = contact.Messages;
            return true;
        }
    }

    public bool GetMessageOfIdBetweenUserAndContact(string userId, string contactId, int messageId, out Message requestedMessage)
    {
        List<Message> msgList;
        if (!GetMessagesBetweenUserAndContact(userId, contactId, out msgList))
        {
            requestedMessage = new Message();
            return false;
        }

        Message? foundMessage = msgList.Find(m => m.Id.Equals(messageId));
        if (null == foundMessage)
        {
            requestedMessage = new Message();
            return false;
        }
        else
        {
            requestedMessage = foundMessage;
            return true;
        }
    }

    public bool UpdateMessageOfIdBetweenUserAndContact(string userId, string contactId, int messageId, string content)
    {
        Message message;

        if (!GetMessageOfIdBetweenUserAndContact(userId, contactId, messageId, out message))
            return false;

        message.Content = content;
        return true;

    }

    public bool SendMessage(string messageContent, string sentFromUserId, string sendToContactId)
    {
        Contact contact;
        if (!GetContactOfId(sentFromUserId, sendToContactId, out contact))
            return false;

        DateTime creationTime = DateTime.Now;

        contact.Messages.Add(new Message()
        {
            Id = ++msgCount,
            Content = messageContent,
            Created = creationTime,
            SentFrom = sentFromUserId,
            SendTo = sendToContactId
        });

        contact.Last = messageContent;
        contact.Lastdate = creationTime.ToString("s");

        return true;
    }

    public bool ReceiveMessage(string messageContent, string sentFromContactId, string sendToUserId)
    {
        Contact contact;
        if (!GetContactOfId(sendToUserId, sentFromContactId, out contact))
            return false;

        DateTime creationTime = DateTime.Now;

        contact.Messages.Add(new Message()
        {
            Id = ++msgCount,
            Content = messageContent,
            Created = creationTime,
            SentFrom = sentFromContactId,
            SendTo = sendToUserId
        });

        contact.Last = messageContent;
        contact.Lastdate = creationTime.ToString("s");

        return true;
    }

    public bool RemoveMessage(string userId, string contactId, int messageId)
    {
        List<Message> msgList = new List<Message>();

        //Returns false if the two IDs aren't contacts of each other
        if (!GetMessagesBetweenUserAndContact(userId, contactId, out msgList))
            return false;

        return msgList.RemoveAll(m => m.Id.Equals(messageId)) > 0;
    }
}
