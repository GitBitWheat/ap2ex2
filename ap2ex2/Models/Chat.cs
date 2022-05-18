namespace ap2ex2.Models
{
    public class Chat
    {
        public Dictionary<string, List<Message>> Messages { get; set; } = new Dictionary<string, List<Message>>();
    }
}
