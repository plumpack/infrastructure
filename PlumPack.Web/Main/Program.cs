using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlumPack.Infrastructure.Data;
using PlumPack.Infrastructure.Migrations;
using Serilog;

namespace PlumPack.Web.Main
{
    public class Program
    {
        private class Options
        {
            [Option('h', "http-port")]
            public int? HttpPort { get; set; }
            
            [Option('c', "config-dir")]
            public string ConfigDirectory { get; set; }
        }
        
        public static void Main<T>(int defaultPort, string additionalConfigDirectory, string[] args, Func<IHost, Task> action) where T: class
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                    .AddJsonFile("logging.json", true)
                    .Build())
                .WriteTo.Console()
                .CreateLogger();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    if (!o.HttpPort.HasValue)
                    {
                        o.HttpPort = defaultPort;
                    }

                    if (string.IsNullOrEmpty(o.ConfigDirectory))
                    {
                        o.ConfigDirectory = additionalConfigDirectory;
                    }
                    
                    var host = CreateHostBuilder<T>(o).Build();

                    // Wait for the database, and perform any migrations.
                    using (var scope = host.Services.CreateScope())
                    {
                        scope.ServiceProvider.GetRequiredService<IDataService>().WaitForDbConnection(TimeSpan.FromSeconds(5));
                        var migrator = scope.ServiceProvider.GetRequiredService<IMigrator>();
                        migrator.Migrate();
                    }
                
                    // Do any other tasks
                    action?.Invoke(host).GetAwaiter().GetResult();
                
                    host.RunAsync().GetAwaiter().GetResult();
                });
        }
        
        private static IHostBuilder CreateHostBuilder<T>(Options options) where T: class
        {
            return Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(builder =>
                {
                    var yaml = Path.Combine(options.ConfigDirectory, "config.yml");
                    if (File.Exists(yaml))
                    {
                        builder.AddYamlFile(yaml);
                    }
                })
                .ConfigureLogging(config =>
                {
                    config.ClearProviders();
                    config.AddSerilog();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var urls = $"http://*:{options.HttpPort}/";
                    Log.Logger.Information($"Listening on: {urls}");
                    webBuilder.UseUrls(urls);
                    webBuilder.UseStartup<T>();
                });
        }
    }
}