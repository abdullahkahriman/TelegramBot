using Newtonsoft.Json;

namespace TelegramBot.Core.Model
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string Surname { get; set; }
    }
}
