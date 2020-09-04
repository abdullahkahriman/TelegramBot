using Newtonsoft.Json;

namespace TelegramBot.Core.Model
{
    public class InlineKeyboardButton
    {
        [JsonProperty(PropertyName = "text")]
        public string text { get; set; }
    }
}
