using Domain;

namespace ap2ex2API.Models
{
    public class MessageApiModel
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public DateTime Created { get; set; }

        public bool Sent { get; set; }

        public MessageApiModel(Message message, string senderId)
        {
            Id = message.Id;
            Content = message.Content;
            Created = message.Created;
            Sent = senderId.Equals(message.SentFrom.Id);
        }
    }
}
