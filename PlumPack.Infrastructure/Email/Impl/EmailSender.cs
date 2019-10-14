using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PlumPack.Infrastructure.Email.Impl
{
    [Service(typeof(IEmailSender))]
    public class EmailSender : IEmailSender
    {
        private readonly PlumPackOptions _plumPackOptions;

        public EmailSender(PlumPackOptions plumPackOptions)
        {
            _plumPackOptions = plumPackOptions;
        }

        public async Task SendEmailAsync(MailServer server, MailAddress from, MailAddress to, string subject, string email, bool html = true)
        {
            var message = new MailMessage(from, to);

            message.IsBodyHtml = html;
            message.Body = email;
            
            using (var client = new SmtpClient())
            {
                client.EnableSsl = server.UseSsl;

                client.Host = server.Host;
                client.Port = server.Port;

                if (server.UseAuthentication)
                {
                    client.UseDefaultCredentials = true;
                    client.Credentials = new NetworkCredential(server.Username, server.Password);
                }
                
                await client.SendMailAsync(message);
            }
        }

        public Task SendEmailAsync(MailAddress to, string subject, string email, bool html = true)
        {
            if (_plumPackOptions.Email == null)
            {
                throw new Exception("Email isn't configured.");
            }

            return SendEmailAsync(new MailServer
                {
                    Host = _plumPackOptions.Email.Host,
                    Port = _plumPackOptions.Email.Port,
                    UseAuthentication = _plumPackOptions.Email.UseAuthentication,
                    Username = _plumPackOptions.Email.Username,
                    Password = _plumPackOptions.Email.Password,
                    UseSsl = _plumPackOptions.Email.UseSsl
                }, new MailAddress(_plumPackOptions.Email.FromEmail, _plumPackOptions.Email.FromName), to, subject,
                email,
                html);
        }
    }
}