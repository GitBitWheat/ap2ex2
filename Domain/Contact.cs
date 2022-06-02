using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Contact
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Server { get; set; }

        public string? Last { get; set; } = null;

        public string? Lastdate { get; set; } = null;

        public List<Message> Messages { get; set; } = new List<Message>();

        public Contact() { }

        public Contact(User user) {
            Id = user.Id;
            Name = user.Name;
            Server = "localhost:5183";
        }
    }
}
