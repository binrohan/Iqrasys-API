using System.Threading.Tasks;
using iqrasys.api.Models;

namespace iqrasys.api.Services
{
    public interface IMailService
    {
         Task SendEmailAsync(MailRequest mailRequest);
    }
}