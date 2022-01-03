using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace iqrasys.api.Models
{

    public class User : IdentityUser
    {
        public DateTime Joined { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

    }

}