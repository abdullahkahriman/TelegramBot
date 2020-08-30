using Newtonsoft.Json;

namespace TelegramBot.Core.Model
{
    public class PollAnswer
    {
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }
    }
}
