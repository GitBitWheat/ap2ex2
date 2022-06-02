using Domain;

namespace ap2ex2API.Models
{
    public class NewContactModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }

        public NewContactModel() { }

        public Contact ConvertToContact()
        {
            return new Contact()
            {
                Id = Id,
                Name = Name,
                Server = Server
            };
        }
    }
}
