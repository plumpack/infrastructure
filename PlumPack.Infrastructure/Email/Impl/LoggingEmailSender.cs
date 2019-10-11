using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PlumPack.Infrastructure.Email.Impl
{
    [Service(typeof(IEmailSender))]
    public class LoggingEmailSender : IEmailSender
    {
        private ILogger<LoggingEmailSender> _logger;
        
        public LoggingEmailSender(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<LoggingEmailSender>();
        }
        
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.LogInformation("Email: {@email} Subject: {@subject} Message: {@htmlMessage}", email, subject, htmlMessage);
            return Task.CompletedTask;
        }
    }
}