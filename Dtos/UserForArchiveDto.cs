using System;

namespace iqrasys.api.Dtos
{
    public class UserForArchiveDto
    {
        public string phoneNumber { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}