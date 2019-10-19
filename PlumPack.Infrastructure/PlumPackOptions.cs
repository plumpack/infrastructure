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
    }
}