using Newtonsoft.Json;

namespace TelegramMovieBot.Core
{
    public class Message
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "chat")]
        public Chat Chat { get; set; }
    }

    public class From
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string Surname { get; set; }
    }

    public class Chat
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string Surname { get; set; }
    }
}