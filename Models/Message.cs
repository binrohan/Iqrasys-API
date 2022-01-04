using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace iqrasys.api.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Text { get; set; }
    }
}