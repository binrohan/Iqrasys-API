using iqrasys.api.Models;

namespace iqrasys.api.Helpers
{
    public static class Utils
    {
        public static string MailBodyHTML(MailRequest mail)
        {
            return $"<p style='font-weight: 600; font-size: 16px; margin: 2px'>Name: {mail.Name ?? "__"}</p><p style='font-weight: 600; font-size: 16px; margin: 2px'>Phone: {mail.Phone ?? "__"}</p><p style='font-weight: 600; font-size: 16px; margin: 2px'>Name: {mail.ToEmail ?? "__"}</p><p style='font-size: 14px'>{mail.Body ?? "__"}</p>";
        }
    }
}