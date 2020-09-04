using Newtonsoft.Json;

namespace TelegramBot.Core.TmdbAPI
{
    public class TmdbResult
    {
        private string imagePathUrl { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "backdrop_path")]
        public string BackdropImagePath
        {
            get { return !string.IsNullOrEmpty(imagePathUrl) ? TmdbApiUrl.TMDB_IMG_URL + imagePathUrl : string.Empty; }
            set { imagePathUrl = value; }
        }

        [JsonProperty(PropertyName = "poster_path")]
        public string PosterImagePath
        {
            get { return !string.IsNullOrEmpty(imagePathUrl) ? (TmdbApiUrl.TMDB_IMG_URL + imagePathUrl) : string.Empty; }
            set { imagePathUrl = value; }
        }
    }
}