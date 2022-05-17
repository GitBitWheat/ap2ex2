﻿using System.ComponentModel.DataAnnotations;

namespace ap2ex2.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public User? sentFrom { get; set; }

        [Required]
        public User? sendTo { get; set; }

        [Required]
        public string? text { get; set; }
    }
}