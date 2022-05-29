using System.Collections.Generic;
using Domain;
namespace Services;

public class UserService : IUserService
{
    private static List<User> users = new List<User>();
    private static int msgCount = 0;

    //Remove later
    public static bool hasBeenInitialized = false;
    public void firstInitialization()
    {
        if (hasBeenInitialized)
            return;

        hasBeenInitialized = true;

        User user1 = new User() { Id = "user1", Name = "First User", Password = "User1" };
        User user2 = new User() { Id = "user2", Name = "Second User", Password = "User2" };
        User user3 = new User() { Id = "user3", Name = "Third User", Password = "User3" };
        User user4 = new User() { Id = "user4", Name = "Fourth User", Password = "User4" };
        users.Add(user1);
        users.Add(user2);
        users.Add(user3);
        users.Add(user4);
        AddContacts("user1", "user2");
        SendMessage("I'm user1", "user1", "user2");
        SendMessage("I'm user2", "user2", "user1");
    }
    //Delete later

    public UserService()
    {
        //Delete later
        firstInitialization();
        //Delete later
    }

    public List<User> GetAllUsers()
    {
        return users;
    }

    public bool GetUser(string id, out User user)
    {
        User? foundUser = users.Find(user => user.Id.Equals(id));
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

    public bool GetContacts(string id, out List<User> contactList)
    {
        User user;
        if (!GetUser(id, out user))
        {
            contactList = new List<User>();
            return false;
        }

        contactList = user.Contacts;
        return true;
    }

    public bool AddContacts(string id1, string id2)
    {
        if (id1.Equals(id2))
        {
            return false;
        }

        User user1, user2;
        if (!GetUser(id1, out user1) || !GetUser(id2, out user2))
        {
            return false;
        }

        if (user1.Contacts.Contains(user2))
        {
             return false;
        }

        user1.Contacts.Add(user2);
        user2.Contacts.Add(user1);
        return true;
    }

    public bool RemoveContacts(string id1, string id2)
    {
        if (id1.Equals(id2))
        {
            return false;
        }

        User user1, user2;
        if (!GetUser(id1, out user1) || !GetUser(id2, out user2))
        {
            return false;
        }

        bool isUser2ContactOfUser1 = user1.Contacts.Remove(user2);
        bool isUser1ContactOfUser2 = user2.Contacts.Remove(user1);
        return isUser2ContactOfUser1 && isUser1ContactOfUser2;
    }

    public bool DoesUserExist(string username)
    {
        User user;
        return GetUser(username, out user);
    }

    public bool IsContactOfUser(string userId, string contactId)
    {
        User contact;
        if (!GetUser(contactId, out contact))
        {
            return false;
        }

        List<User> contactsList;
        if (GetContacts(userId, out contactsList))
            return contactsList.Contains(contact);
        else
            return false;
    }

    public bool GetMessagesBetweenTwoUsers(string userId, string contactId, out List<Message> msgList)
    {
        User user;
        if (!GetUser(userId, out user) || !IsContactOfUser(userId, contactId))
        {
            msgList = new List<Message>();
            return false;

        }

        if (!user.Messages.TryGetValue(contactId, out msgList))
            msgList = new List<Message>();

        return true;
    }

    public bool GetMessageOfIdBetweenTwoUsers(string userId1, string userId2, int messageId, out Message requestedMessage)
    {
        List<Message> msgList = new List<Message>();
        if (!GetMessagesBetweenTwoUsers(userId1, userId2, out msgList))
        {
            requestedMessage = new Message();
            return false;
        }

        Message? foundMessage = msgList.Find(message => { return message.Id == messageId; });
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

    public bool SendMessage(string message, string sentFromId, string sendToId)
    {
        User sentFromUser, sendToUser;

        if (!GetUser(sentFromId, out sentFromUser) || !GetUser(sendToId, out sendToUser))
            return false;

        List<Message> msgList;
        if (!sentFromUser.Messages.TryGetValue(sendToUser.Id, out msgList))
        {
            msgList = new List<Message>();
            sentFromUser.Messages.Add(sendToUser.Id, msgList);
            sendToUser.Messages.Add(sentFromUser.Id, msgList);
        }

        DateTime creationTime = DateTime.Now;
        msgList.Add(new Message()
        {
            Id = ++msgCount,
            Content = message,
            Created = creationTime,
            SentFrom = sentFromUser,
            SendTo = sendToUser
        });

        return true;
    }

    public bool RemoveMessage(string userId1, string userId2, int messageId)
    {
        List<Message> msgList = new List<Message>();
        if (!GetMessagesBetweenTwoUsers(userId1, userId2, out msgList))
            return false;

        Message? foundMessage = msgList.Find(message => { return message.Id == messageId; });
        if (null == foundMessage)
            return false;
        
        return msgList.Remove(foundMessage);
    }
}