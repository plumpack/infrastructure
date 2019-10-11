using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PlumPack.IdentityServer.Client
{
    public static class IdentityServerClientExtensions
    {
        public static void AddIdentityServerClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            var identityServerClientOptions = IdentityServerClientOptions.Bind(configuration);
            services.AddSingleton(identityServerClientOptions);
            
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = identityServerClientOptions.IdentityServerUrl;
                    options.RequireHttpsMetadata = false;

                    options.ClientId = identityServerClientOptions.ClientId;
                    options.ClientSecret = identityServerClientOptions.Secret;
                    options.ResponseType = "code id_token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("offline_access");
                    options.Scope.Add("email");
                });
        }

        public static IApplicationBuilder UseIdentityServerClient(this IApplicationBuilder builder)
        {
            return builder.UseEndpoints(routes =>
            {
                routes.MapGet("/logout", ProcessLogout);
                routes.MapGet("/logout/callback", ProcessLogoutCallback);
                routes.MapGet("/logout/frontchannel", ProcessLogoutFrontChannel);
            });
        }

        private static async Task ProcessLogout(HttpContext context)
        {
            context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
            context.Response.Headers.Add("Pragma", "no-cache");
            
            var identityServerUrl = context.RequestServices.GetRequiredService<IdentityServerClientOptions>()
                .IdentityServerUrl;
            
            var idToken = await context.GetTokenAsync("id_token");

            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(identityServerUrl);
                if (disco.IsError) throw new Exception(disco.Error);

                var endSessionUrl =
                    $"{disco.EndSessionEndpoint}?id_token_hint={idToken}&post_logout_redirect_uri={context.Request.Scheme}://{context.Request.Host}/logout/callback";

                context.Response.Redirect(endSessionUrl);
            }

            await context.Response.CompleteAsync();
        }
        
        private static async Task ProcessLogoutCallback(HttpContext context)
        {
            context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
            context.Response.Headers.Add("Pragma", "no-cache");
            
            context.Response.Redirect("/");
            
            await context.Response.CompleteAsync();
        }

        private static async Task ProcessLogoutFrontChannel(HttpContext context)
        {
            context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
            context.Response.Headers.Add("Pragma", "no-cache");

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var sid = context.Request.Query["sid"];
                var currentSid = context.User.FindFirst("sid")?.Value ?? "";
                if (string.Equals(currentSid, sid, StringComparison.Ordinal))
                {
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }

            await context.Response.CompleteAsync();
        }
    }
}