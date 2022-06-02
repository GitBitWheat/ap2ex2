using Domain;

namespace ap2ex2API.Models
{
    public class ContactApiModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public string? Last { get; set; }
        public string? Lastdate { get; set; }

        public ContactApiModel() { }

        public ContactApiModel(Contact contact)
        {
            Id = contact.Id;
            Name = contact.Name;
            Server = contact.Server;
            Last = contact.Last;
            Lastdate = contact.Lastdate;
        }
    }
}
