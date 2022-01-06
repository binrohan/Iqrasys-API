using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using iqrasys.api.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace iqrasys.api.Services
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}