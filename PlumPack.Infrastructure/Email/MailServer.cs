namespace PlumPack.Infrastructure.Email
{
    public class MailServer
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public bool UseSsl { get; set; }

        public bool UseAuthentication { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}