using System;
using System.ComponentModel.DataAnnotations;

namespace iqrasys.api.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8,MinimumLength = 4, ErrorMessage = "You Must Input 4 character password")]
        public string Password { get; set; }
        [Required]
        public DateTime Joined { get; set; }
        public string PhoneNumber { get; set; }

        public UserForRegisterDto()
        {
            Joined =  DateTime.Now;
        }
    }
}