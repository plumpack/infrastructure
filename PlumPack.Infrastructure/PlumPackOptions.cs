namespace PlumPack.Infrastructure
{
    public class PlumPackOptions
    {
        public PlumPackOptions()
        {
            MainSiteUrl = "https://plumpack.com/";
        }
        
        /// <summary>
        /// There are a number of sub-sites that make of PlumPack.
        /// But only one sub-site is considered "home".
        /// Typically https://plumpack.com/
        /// </summary>
        public string MainSiteUrl { get; set; }
        
        public EmailOptions Email { get; set; }
        
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
        }

    }
}