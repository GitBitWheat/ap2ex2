using System.ComponentModel.DataAnnotations;

namespace ap2ex2.Models
{
    public class User
    {
        [Key]
        public string? Userame { get; set; }

        [Required]
        public string? Nickname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        public string? Pfp { get; set; }

        public List<List<Message>>? Messages { get; set; } = new List<List<Message>>();
    }
}
