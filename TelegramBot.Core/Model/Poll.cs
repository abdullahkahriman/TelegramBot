using Newtonsoft.Json;
using System.Collections.Generic;

namespace TelegramBot.Core.Model
{
    public class Poll
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }

        [JsonProperty(PropertyName = "options")]
        public List<PollOption> Options { get; set; }
    }
}