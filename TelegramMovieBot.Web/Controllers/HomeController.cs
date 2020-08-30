using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TelegramMovieBot.Core;

namespace TelegramMovieBot.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Telegram telegram = new Telegram(Static.TOKEN);
            telegram.SetWebhookAsync("https://49c665081276.ngrok.io");

            var message = telegram.GetChat(Request.Body);
            telegram.ExplodeAsync(message);

            return View();
        }
    }
}