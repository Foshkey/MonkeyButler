using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MonkeyButler
{
    /// <summary>
    /// Entry.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}