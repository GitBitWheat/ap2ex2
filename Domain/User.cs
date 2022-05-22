﻿using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "A username is required")]
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        public string Id { get; set; }

        [Required(ErrorMessage = "A nickname is required")]
        [DataType(DataType.Text)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "A password is required")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).+$", ErrorMessage = "Passwords contain at least 1 lower case letter, 1 uppercase letter and 1 digit")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public List<User> Contacts { get; set; } = new List<User>();

        public Dictionary<string, List<Message>> MessagesD { get; set; } = new Dictionary<string, List<Message>>(StringComparer.Ordinal);

        public User UserInChat { get; set; }
    }
}