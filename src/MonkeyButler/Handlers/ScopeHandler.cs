using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MonkeyButler.Options;

namespace MonkeyButler.Handlers
{
    internal class ScopeHandler : IScopeHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptionsMonitor<AppOptions> _appOptions;

        private readonly ConcurrentDictionary<ulong, IServiceScope> _serviceScopes = new ConcurrentDictionary<ulong, IServiceScope>();

        public ScopeHandler(IServiceProvider serviceProvider, IOptionsMonitor<AppOptions> appOptions)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _appOptions = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
        }

        public IServiceScope CreateScope(ulong id)
        {
            // Managing the scope manually like this is necessary because awaiting ExecuteAsync
            // truly does not wait until the command is complete. We need OnExecuted for that.
            var scope = _serviceProvider.CreateScope();
            _serviceScopes.AddOrUpdate(id, scope, (id, newScope) => newScope);
            _ = StartCleanupTimer(id);

            return scope;
        }

        public Task OnCommandExecuted(Optional<CommandInfo> commandInfo, ICommandContext context, IResult result)
        {
            RemoveScope(context.Message.Id);
            return Task.CompletedTask;
        }

        // Just in case OnExecuted is never called for whatever reason...
        private async Task StartCleanupTimer(ulong id)
        {
            var delayTime = _appOptions.CurrentValue.Discord?.ScopeCleanupDelay ?? new TimeSpan(0, 1, 0);
            await Task.Delay(delayTime);
            RemoveScope(id);
        }

        private void RemoveScope(ulong id)
        {
            if (_serviceScopes.TryRemove(id, out var scope))
            {
                scope.Dispose();
            }
        }
    }

    internal interface IScopeHandler
    {
        IServiceScope CreateScope(ulong id);
        Task OnCommandExecuted(Optional<CommandInfo> commandInfo, ICommandContext context, IResult result);
    }
}
