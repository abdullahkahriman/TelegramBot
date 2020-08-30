using Newtonsoft.Json;

namespace TelegramBot.Core.Model
{
    public class PollOption
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "voter_count")]
        public int VoterCount { get; set; }
    }
}
