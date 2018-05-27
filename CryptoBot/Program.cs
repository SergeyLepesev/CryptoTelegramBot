using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var appSettings = new AppSettings();
                    context.Configuration.Bind(appSettings);
                    services.AddSingleton(appSettings);
                })
                .UseStartup<Startup>()
                .Build();
    }
}