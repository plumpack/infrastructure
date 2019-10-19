using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace PlumPack.Infrastructure.Email.Impl
{
    [Service(typeof(IEmailSender))]
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions _emailOptions;

        public EmailSender(IOptions<EmailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
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
            _emailOptions.AssertValid();
            
            return SendEmailAsync(new MailServer
                {
                    Host = _emailOptions.Host,
                    Port = _emailOptions.Port,
                    UseAuthentication = _emailOptions.UseAuthentication,
                    Username = _emailOptions.Username,
                    Password = _emailOptions.Password,
                    UseSsl = _emailOptions.UseSsl
                }, new MailAddress(_emailOptions.FromEmail, _emailOptions.FromName), to, subject,
                email,
                html);
        }
    }
}