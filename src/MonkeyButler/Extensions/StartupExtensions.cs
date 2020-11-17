using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Data.Database;

namespace MonkeyButler.Extensions
{
    internal static class StartupExtensions
    {
        internal static void UpdateDatabase(this IApplicationBuilder app)
        {
            using var migrationScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            using var context = migrationScope.ServiceProvider.GetService<MonkeyButlerContext>();

            context.Database.Migrate();
        }
    }
}
