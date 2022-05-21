using System.ComponentModel.DataAnnotations;

namespace ap2ex2.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "A username is required")]
        [DataType(DataType.Text)]
        public string? Username { get; set; }

        [Required(ErrorMessage = "A nickname is required")]
        [DataType(DataType.Text)]
        public string? Nickname { get; set; }

        [Required(ErrorMessage = "A password is required")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).+$", ErrorMessage = "Passwords contain at least 1 lower case letter, 1 uppercase letter and 1 digit")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Profile picture")]
        [DataType(DataType.ImageUrl)]
        public string? Pfp { get; set; }

        public List<User> Contacts { get; set; } = new List<User>();
        
        public List<Message> Messages { get; set; } = new List<Message>();
        public User UserInChat { get; set; }
    }
}
