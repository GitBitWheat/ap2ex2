using System.ComponentModel.DataAnnotations;

namespace ap2ex2.Models
{
    public class Message
    {
        [Required]
        public User? sentFrom { get; set; }

        [Required]
        public User? sendTo { get; set; }

        [Required]
        public string? text { get; set; }
        public DateTime dateTime { get; set; }
    }
}
