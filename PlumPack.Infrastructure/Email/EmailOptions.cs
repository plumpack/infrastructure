using System;

namespace PlumPack.Infrastructure.Email
{
    public class EmailOptions
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public bool UseSsl { get; set; }

        public bool UseAuthentication { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FromName { get; set; }

        public string FromEmail { get; set; }

        public void AssertValid()
        {
            if (string.IsNullOrEmpty(Host))
            {
                throw new Exception("No email hostname provided");
            }

            if (string.IsNullOrEmpty(FromEmail))
            {
                throw new Exception("No email from address");
            }
        }
    }
}