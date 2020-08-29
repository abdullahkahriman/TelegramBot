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
            Telegram telegram = new Telegram("1335790071:AAHjvauoB_TTwxaYBtBld_XA7znIvL_Iyis");
            //telegram.SetWebhookAsync("https://73056832c009.ngrok.io");

            var message = telegram.GetData(Request.Body);

            telegram.SendMessageAsync("selam, bu mesaj otomatik olarak yazıldı.");
            return View();
        }
    }
}