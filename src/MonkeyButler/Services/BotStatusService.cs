using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace MonkeyButler.Services
{
    internal class BotStatusService : IBotStatusService
    {
        private readonly DiscordSocketClient _client;

        public event Func<Task> OnUpdated;

        public BotStatusService(DiscordSocketClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));

            _client.Connected += OnConnected;
            _client.Disconnected += OnDisconnected;
            _client.LatencyUpdated += OnLatencyUpdated;

            OnUpdated += () => Task.CompletedTask;
        }

        public ConnectionState ConnectionState => _client.ConnectionState;
        public int Latency => _client.Latency;

        private Task OnConnected() => OnUpdated.Invoke();
        private Task OnDisconnected(Exception arg) => OnUpdated.Invoke();
        private Task OnLatencyUpdated(int arg1, int arg2) => OnUpdated.Invoke();

        public void Dispose()
        {
            _client.Connected -= OnConnected;
            _client.Disconnected -= OnDisconnected;
            _client.LatencyUpdated -= OnLatencyUpdated;
        }
    }

    internal interface IBotStatusService : IDisposable
    {
        ConnectionState ConnectionState { get; }
        int Latency { get; }

        event Func<Task> OnUpdated;
    }
}
