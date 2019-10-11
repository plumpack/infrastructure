using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;

namespace PlumPack.IdentityServer.Client
{
    public class IdentityServerClientOptions
    {
        public string IdentityServerUrl { get; set; }
        
        public string ClientId { get; set; }
        
        public string Secret { get; set; }

        public static IdentityServerClientOptions Bind(IConfiguration configuration)
        {
            var result = new IdentityServerClientOptions();
            var section = configuration.GetSection("IdentityServerClient");
            if (!section.Exists())
            {
                throw new Exception("IdentityServerClient options don't exist.");
            }
            section.Bind(result);
            if(string.IsNullOrEmpty(result.IdentityServerUrl))
            {
                throw new Exception("IdentityServerClient.IdentityServerUrl wasn't provided.");
            }
            if (string.IsNullOrEmpty(result.ClientId))
            {
                throw new Exception("IdentityServerClient.ClientId wasn't provided.");
            }
            if (string.IsNullOrEmpty(result.Secret))
            {
                throw new Exception("IdentityServerClient.Secret wasn't provided.");
            }
            return result;
        }
    }
}