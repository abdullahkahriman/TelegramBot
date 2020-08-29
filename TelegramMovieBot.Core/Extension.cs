using Newtonsoft.Json;

namespace TelegramMovieBot.Core
{
    public static class Extension
    {
        public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj);
    }
}