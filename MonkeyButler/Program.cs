using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MonkeyButler.Config;
using MonkeyButler.Diagnostics;
using MonkeyButler.Handlers;
using System.Reflection;
using System.Threading.Tasks;

namespace MonkeyButler {
    internal class Program {
        public static void Main(string[] args) {
            var serviceProvider = new Services().BuildServiceProvider();
            var program = serviceProvider.GetService<Program>();
            program.RunAsync().GetAwaiter().GetResult();
        }

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly Credentials _credentials;
        private readonly DiscordLogger _discordLogger;
        private readonly MessageHandler _messageHandler;
        private readonly UserJoinedHandler _userJoinedHandler;

        public Program(DiscordSocketClient client, CommandService commands, Credentials credentials, DiscordLogger discordLogger, MessageHandler messageHandler, UserJoinedHandler userJoinedHandler) {
            _client = client;
            _commands = commands;
            _credentials = credentials;
            _discordLogger = discordLogger;
            _messageHandler = messageHandler;
            _userJoinedHandler = userJoinedHandler;
        }

        public async Task RunAsync() {
            RegisterEvents();
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());

            await _client.LoginAsync(TokenType.Bot, _credentials.Token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private void RegisterEvents() {
            _client.Log += _discordLogger.Log;
            _client.MessageReceived += _messageHandler.HandleMessage;
            _client.UserJoined += _userJoinedHandler.HandleUser;
        }
    }
}
