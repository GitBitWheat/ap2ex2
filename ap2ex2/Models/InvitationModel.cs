using System.ComponentModel.DataAnnotations;

namespace ap2ex2.Models
{
    public class InvitationModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Server { get; set; }
    }
}