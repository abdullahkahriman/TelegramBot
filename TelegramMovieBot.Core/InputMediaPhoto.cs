namespace TelegramMovieBot.Core
{
    public class InputMediaPhoto
    {
        public string type { get; set; } = "photo";

        public string media { get; set; }

        public string caption { get; set; } = string.Empty;
    }
}