using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace PlumPack.Web
{
    public static class AppBuilderExtensions
    {
        public static PlumPackExtensions PlumPack(this IApplicationBuilder builder, IWebHostEnvironment env)
        {
            return new PlumPackExtensions {AppBuilder = builder, Env = env};
        }

        public class PlumPackExtensions
        {
            public IApplicationBuilder AppBuilder { get; set; }
            
            public IWebHostEnvironment Env { get; set; }
        }
        
        public static PlumPackExtensions UseExceptionPage(this PlumPackExtensions app)
        {
            if (app.Env.IsDevelopment())
            {
                app.AppBuilder.UseDeveloperExceptionPage();
            }
            else
            {
                app.AppBuilder.UseExceptionHandler("/error");
            }

            return app;
        }
        
        public static PlumPackExtensions UseStaticFiles(this PlumPackExtensions app)
        {
            if (app.Env.IsDevelopment())
            {
                app.AppBuilder.UseStaticFiles(new StaticFileOptions()
                {
                    OnPrepareResponse = (context) =>
                    {
                        // Disable caching of all static files.
                        context.Context.Response.Headers["Cache-Control"] = "no-cache, no-store";
                        context.Context.Response.Headers["Pragma"] = "no-cache";
                        context.Context.Response.Headers["Expires"] = "-1";
                    }
                });
            }
            else
            {
                app.AppBuilder.UseStaticFiles();
            }

            return app;
        }
    }
}