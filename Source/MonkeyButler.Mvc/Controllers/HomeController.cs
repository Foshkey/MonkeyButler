using System;
using Discord;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonkeyButler.Mvc.Models.Home;

namespace MonkeyButler.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDiscordClient _discordClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IDiscordClient discordClient, ILogger<HomeController> logger)
        {
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            _logger.LogTrace("Loading home page.");

            return View(new IndexModel()
            {
                ConnectionState = _discordClient.ConnectionState
            });
        }
    }
}
