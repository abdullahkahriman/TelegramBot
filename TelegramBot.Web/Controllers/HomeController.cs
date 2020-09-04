using Microsoft.AspNetCore.Mvc;
using TelegramBot.Core;
using TelegramBot.Core.Model;

namespace TelegramBot.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Telegram telegram = new Telegram(Static.TOKEN);
            telegram.SetWebhookAsync("https://3042cb1b504e.ngrok.io");

            Message message = telegram.GetChat(Request.Body);
            if (message != null)
                telegram.ExplodeAsync(message);

            return View();
        }
    }
}