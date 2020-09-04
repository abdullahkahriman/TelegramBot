using Newtonsoft.Json;

namespace TelegramBot.Core.Model
{
    public class KeyboardButton
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "callback_data")]
        public string CallbackData { get; set; }
    }
}
