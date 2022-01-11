using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace iqrasys.api.Models
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }

        public MailRequest()
        {
            ToEmail = "binrohan.cs@gmail.com";
        }
    }
}