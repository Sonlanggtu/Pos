using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Pos.Gateway
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; } = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                   .AddJsonFile("gateways.json")
                   .Build();

        public static void Main(string[] args)
        {
            try
            {
                string logTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u}] [{SourceContext}] {Message}{NewLine}{Exception}";
                var dateTime = DateTime.Now;
                string dateString = dateTime.ToString("dd-MM-yyyy");

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: logTemplate)
                    .WriteTo.File($"log-{dateString}.txt", outputTemplate: logTemplate)
                    .CreateLogger();
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((host, config) =>
                {
                    config.AddConfiguration(Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
