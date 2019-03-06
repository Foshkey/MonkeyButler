using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace MonkeyButler.Bot.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            using (_logger.BeginScope("Referrer:{Referrer}", HttpContext.Request.Headers[HeaderNames.Referer].ToString()))
            {
                _logger.LogWarning("Error page requested.");
                return View();
            }
        }
    }
}
