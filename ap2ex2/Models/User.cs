using System.ComponentModel.DataAnnotations;

namespace ap2ex2.Models
{
    public class User
    {
        [Key]
        public int? Id { get; set; }
        
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Nickname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string? Pfp { get; set; }

        public List<List<Message>>? Messages { get; set; }
    }
}
