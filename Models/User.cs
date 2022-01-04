using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace iqrasys.api.Models
{

    public class User : IdentityUser
    {
        public DateTime Joined { get; set; }
    }

}