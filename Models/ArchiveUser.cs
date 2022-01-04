using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace iqrasys.api.Models
{

    public class ArchiveUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Joined { get; set; }
        public DateTime RemoveDate { get; set; }

        public ArchiveUser()
        {
            RemoveDate = DateTime.Now;
        }
    }

}