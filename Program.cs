// Program.cs
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BibliotecaApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.ConfigureWebHostDefaults(delegate (Microsoft.AspNetCore.Hosting.IWebHostBuilder webBuilder)
            {
                webBuilder.UseStartup<Startup>();
            });
            IHost host = hostBuilder.Build();
            host.Run();
        }
    }
}
