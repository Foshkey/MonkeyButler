using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Bot;

namespace MonkeyButler.Mvc
{
    public static class ApplicationBuilderExtensions
    {
        public static Task StartBot(this IApplicationBuilder app) => app.ApplicationServices.GetRequiredService<IBot>().StartAsync();
    }
}
