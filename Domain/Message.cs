using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Message
    {
        [Required]
        public User sentFrom { get; set; }

        [Required]
        public User sendTo { get; set; }

        [Required]
        public string? text { get; set; }
    }
}