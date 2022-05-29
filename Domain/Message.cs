using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Content { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public User SentFrom { get; set; }

        [Required]
        public User SendTo { get; set; }
    }
}