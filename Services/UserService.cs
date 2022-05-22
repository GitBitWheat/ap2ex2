using System.Collections.Generic;
using Domain;
namespace Services;

public class UserService : IUserService
{
    private static List<User> users = new List<User>();

    public UserService()
    {
        User user1 = new User() { Id = "user1", Name = "First User", Password = "User1" };
        User user2 = new User() { Id = "user2", Name = "Second User", Password = "User2" };
        User user3 = new User() { Id = "user3", Name = "Third User", Password = "User3" };
        User user4 = new User() { Id = "user4", Name = "Fourth User", Password = "User4" };
        users.Add(user1);
        users.Add(user2);
        users.Add(user3);
        users.Add(user4);
        AddContacts("user1", "user4");
        SendMessage("hi", "user1", "user4");
    }

public List<User> GetAllUsers()
    {
        return users;
    }

    public User? GetUser(string id)
    {
        return users.Find(user => user.Id.Equals(id));
    }

    public void AddUser(User user)
    {
        users.Add(user);
    }

    public bool Login(string username, string password)
    {
        User? user = GetUser(username);
        if (user != null && user.Password == password)
        {
            return true;
        }

        return false;
    }

    public List<User>? GetContacts(string id)
    {
        User? user = GetUser(id);
        if (null == user)
            return null;
        else
            return user.Contacts;
    }

    public void AddContacts(string id1, string id2)
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

    public bool doesUserExist(string username)
    {
        return GetUser(username) != null;
    }

    public void addMessage(Message message)
    {
        User? sentFromUser = GetUser(message.sentFrom.Id);
    }

    public bool isContactOfUser(string userId, string contactId)
    {
        User? contact = GetUser(contactId);
        List<User>? contactsList = GetContacts(userId);
        if (null == contactsList || null == contact)
            return false;
        else
        {
            return contactsList.Contains(contact);
        }
    }

    public List<Message>? getMessagesBetweenTwoUsers(string id1, string id2)
    {
        User? user1 = GetUser(id1);
        if (null == user1)
            return null;
        List<Message>? messageList;
        if (user1.MessagesD.TryGetValue(id2, out messageList))
            return messageList;
        else
            return null;
    }

    public void SendMessage(string message, string sentFromId, string sendToId)
    {
        User sentFromUser = GetUser(sentFromId);
        User sendToUser = GetUser(sendToId);

        if (!sentFromUser.MessagesD.ContainsKey(sendToUser.Id))
        {
            sentFromUser.MessagesD.Add(sendToUser.Id, new List<Message>());
            sendToUser.MessagesD.Add(sentFromUser.Id, new List<Message>());
        }

        sentFromUser.MessagesD[sendToUser.Id].Add(new Message()
        {
            sentFrom = sentFromUser,
            sendTo = sendToUser,
            text = message,
            dateTime = DateTime.Now
        });

        sendToUser.MessagesD[sentFromUser.Id].Add(new Message()
        {
            sentFrom = sentFromUser,
            sendTo = sendToUser,
            text = message,
            dateTime = DateTime.Now
        });
    }
}