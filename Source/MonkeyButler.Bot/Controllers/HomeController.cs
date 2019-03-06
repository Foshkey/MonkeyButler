using System;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonkeyButler.Bot.Models.Home;

namespace MonkeyButler.Bot.Controllers
{
    public class HomeController : Controller
    {
        private readonly DiscordSocketClient _discord;
        private readonly ILogger<HomeController> _logger;

        public HomeController(DiscordSocketClient discord, ILogger<HomeController> logger)
        {
            _discord = discord ?? throw new ArgumentNullException(nameof(discord));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            _logger.LogTrace("Loading home page.");

            return View(new IndexModel()
            {
                ConnectionStatus = _discord.ConnectionState.ToString()
            });
        }
    }
}
