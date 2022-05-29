using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Contact
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }

        public Contact() { }
        public Contact(User user) {
            Id = user.Id;
            Name = user.Name;
            Server = user.Server;
        }
    }
}
