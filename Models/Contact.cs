using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace iqrasys.api.Models
{
    public class Contact
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}