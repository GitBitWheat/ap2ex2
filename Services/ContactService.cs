using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public class ContactService : IContactService
    {
        private static Dictionary<string, List<Contact>> contactsDict = new Dictionary<string, List<Contact>>(StringComparer.Ordinal);
        private static Dictionary<Tuple<string, string>, List<Message>> messagesDict
            = new Dictionary<Tuple<string, string>, List<Message>>(new UnorderedStringPairEqualityComparer());
        private static int msgCount = 0;


        public List<Contact> GetContacts(string id)
        {
            List<Contact>? contactList = new List<Contact>();
            if (contactsDict.TryGetValue(id, out contactList))
                return contactList;
            else
                return new List<Contact>();
        }

        public bool GetContactOfId(string userId, string contactId, out Contact requestedContact)
        {
            Contact? foundContact = GetContacts(userId).Find(c => c.Id.Equals(contactId));
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

        public bool AddContact(string userId, Contact newContact)
        {
            if (userId.Equals(newContact.Id))
            {
                return false;
            }

            if (contactsDict.ContainsKey(userId))
            {
                if (IsContactOfUser(userId, newContact.Id))
                    return false;
            }
            else
                contactsDict[userId] = new List<Contact>();

            contactsDict[userId].Add(newContact);

            Tuple<string, string> contactsTuple = new Tuple<string, string>(userId, newContact.Id);
            if (!messagesDict.ContainsKey(contactsTuple))
            {
                messagesDict[contactsTuple] = new List<Message>();
            }

            return true;
        }

        public bool RemoveContact(string userId, string contactId)
        {
            if (!contactsDict.ContainsKey(userId))
                return false;

            if (!IsContactOfUser(userId, contactId))
                return false;

            contactsDict[userId].RemoveAll(c => c.Id == contactId);
            return true;
        }

        public bool IsContactOfUser(string userId, string contactId)
        {
            return contactsDict[userId].Exists(c => c.Id.Equals(contactId));
        }

        public bool GetMessagesBetweenContacts(string contactId1, string contactId2, out List<Message> msgList)
        {
            if (!IsContactOfUser(contactId1, contactId2))
            {
                msgList = new List<Message>();
                return false;
            }

            Tuple<string, string> contactsTuple = new Tuple<string, string>(contactId1, contactId2);
            if (messagesDict.ContainsKey(contactsTuple))
                msgList = messagesDict[contactsTuple];
            else
                msgList = new List<Message>();

            return true;
        }

        public bool GetMessageOfIdBetweenContacts(string contactId1, string contactId2, int messageId, out Message requestedMessage)
        {
            List<Message> msgList = new List<Message>();

            //Returns false if the two IDs aren't contacts of each other
            if (!GetMessagesBetweenContacts(contactId1, contactId2, out msgList))
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

        public bool SendMessage(string messageContent, string sentFromId, string sendToId)
        {
            if (!IsContactOfUser(sentFromId, sendToId))
                return false;

            messagesDict[new Tuple<string, string>(sentFromId, sendToId)].Add(new Message()
            {
                Id = ++msgCount,
                Content = messageContent,
                Created = DateTime.Now,
                SentFrom = sentFromId,
                SendTo = sendToId
            });
            return true;
        }

        public bool RemoveMessage(string contactId1, string contactId2, int messageId)
        {
            List<Message> msgList = new List<Message>();

            //Returns false if the two IDs aren't contacts of each other
            if (!GetMessagesBetweenContacts(contactId1, contactId2, out msgList))
                return false;

            if (msgList.Exists(m => m.Id.Equals(messageId))) {
                msgList.RemoveAll(m => m.Id.Equals(messageId));
                return true;
            }
            else
            {
                return false;
            }
        }



        private class UnorderedStringPairEqualityComparer : IEqualityComparer<Tuple<string, string>>
        {
            public bool Equals(Tuple<string, string>? x, Tuple<string, string>? y)
            {
                if (x != null && y != null)
                {
                    return (x.Item1.Equals(y.Item1) && x.Item2.Equals(y.Item2))
                        || (x.Item1.Equals(y.Item2) && x.Item2.Equals(y.Item1));
                }
                else if (x == null && y == null)
                    return true;
                else
                    return false;
            }

            public int GetHashCode([DisallowNull] Tuple<string, string> obj)
            {
                if (String.Compare(obj.Item1, obj.Item2, comparisonType: StringComparison.Ordinal) >= 0)
                {
                    return obj.GetHashCode();
                }
                else
                {
                    return new Tuple<string, string>(obj.Item2, obj.Item1).GetHashCode();
                }
            }
        }
    }
}
