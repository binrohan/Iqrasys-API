using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace iqrasys.api.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [MinLength(25, ErrorMessage = "Minimum 50 characters required")]
        public string Text { get; set; }
        public DateTime PostDate { get; set; }
        public bool IsTrashed { get; set; }

        public Message()
        {
            PostDate = DateTime.Now;
            IsTrashed = false;
        }
    }
}