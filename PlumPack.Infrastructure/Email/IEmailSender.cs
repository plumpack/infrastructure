using System.Net.Mail;
using System.Threading.Tasks;

namespace PlumPack.Infrastructure.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MailServer server, MailAddress from, MailAddress to, string subject, string email, bool html = true);
        
        Task SendEmailAsync(MailAddress to, string subject, string email, bool html = true);
    }
}