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
            //telegram.SetWebhookAsync("https://73056832c009.ngrok.io");

            var message = telegram.GetChat(Request.Body);
            telegram.Explode(message);


            //telegram.SendMessageAsync("selam, bu mesaj otomatik olarak yazıldı.");
            return View();
        }
    }
}