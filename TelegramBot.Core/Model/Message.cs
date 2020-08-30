using Newtonsoft.Json;
using TelegramBot.Core.Model;

namespace TelegramBot.Core.Model
{
    public class Message
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "chat")]
        public User Chat { get; set; }
    }
}