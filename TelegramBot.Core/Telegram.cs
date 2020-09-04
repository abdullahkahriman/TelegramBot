using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Core.Model;
using TelegramBot.Core.TmdbAPI;
using static TelegramBot.Core.Static;

namespace TelegramBot.Core
{
    public class Telegram
    {
        private const string API_URL = "https://api.telegram.org/bot";
        private string _token;
        private string _chatID;
        private static string webhookUrl;

        public Telegram(string token)
        {
            this._token = token;
        }

        public async void SetWebhookAsync(string url)
        {
            if (string.IsNullOrEmpty(webhookUrl))
            {
                var obj = new
                {
                    url = url
                };
                webhookUrl = url;
                await RequestAsync("setWebhook", obj);
            }
        }

        public async void SendMessageAsync(string message, ReplyKeyboardMarkup markup = null)
        {
            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(this._chatID))
            {
                var obj = new
                {
                    chat_id = this._chatID,
                    text = message,
                    reply_markup = markup != null ? markup.ToJson() : ""
                };
                await RequestAsync("sendMessage", obj);
            }
        }

        public async void SendPhotoAsync(string photoUrl, string caption = "")
        {
            if (!string.IsNullOrEmpty(photoUrl) && !string.IsNullOrEmpty(this._chatID))
            {
                var obj = new
                {
                    chat_id = this._chatID,
                    photo = photoUrl,
                    caption = caption
                };
                await RequestAsync("sendPhoto", obj);
            }
        }

        /// <summary>
        /// Min 2, Max 10 photo
        /// </summary>
        /// <param name="inputMediaPhotos"></param>
        public async void SendMediaPhotoGroupAsync(List<InputMediaPhoto> inputMediaPhotos)
        {
            if (inputMediaPhotos != null && inputMediaPhotos.Count >= 1 &&
                !string.IsNullOrEmpty(this._chatID))
            {
                var obj = new
                {
                    chat_id = this._chatID,
                    media = (inputMediaPhotos.Count > 9 ? inputMediaPhotos.Take(9) : inputMediaPhotos).ToJson()
                };
                await RequestAsync("sendMediaGroup", obj);
            }
        }

        async void SendChatActionAsync(ChatAction chatAction)
        {
            if (!string.IsNullOrEmpty(this._chatID))
            {
                var obj = new
                {
                    chat_id = this._chatID,
                    action = chatAction.GetDescription()
                };
                await RequestAsync("sendChatAction", obj);
            }
        }

        async void SendDiceAsync()
        {
            if (!string.IsNullOrEmpty(this._chatID))
            {
                var obj = new
                {
                    chat_id = this._chatID
                };
                await RequestAsync("sendDice", obj);
            }
        }

        async void SendPollAsync(string question, string[] options, bool isAnonymous = true)
        {
            if (!string.IsNullOrEmpty(this._chatID))
            {
                var obj = new
                {
                    chat_id = this._chatID,
                    question = question,
                    is_anonymous = isAnonymous,
                    options = options.ToJson()
                };
                await RequestAsync("sendPoll", obj);
            }
        }

        private string GetStreamReader(Stream stream)
        {
            using (StreamReader streamReader = new StreamReader(stream))
            {
                string response = streamReader.ReadToEnd();
                return response;
            }
        }

        public Message GetChat(Stream stream)
        {
            try
            {
                string response = GetStreamReader(stream);
                JObject jObject = JObject.Parse(response);
                Message message = jObject.SelectToken("message")?.ToObject<Message>();
                this._chatID = message?.Chat?.ID;

                //Poll poll = jObject.SelectToken("poll")?.ToObject<Poll>();

                PollAnswer pollAnswer = jObject.SelectToken("poll_answer")?.ToObject<PollAnswer>();

                if (pollAnswer != null)
                {
                    this._chatID = pollAnswer.User.ID;
                    SendMessageAsync("Teşekkürler");
                }
                return message;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async void ExplodeAsync(Message message)
        {
            string result = string.Empty;

            if (message != null)
            {
                string messageText = message.Text?.ToLower();
                switch (messageText)
                {
                    case "/start":
                        result = string.Format(@"Hoş geldin {0},
Film önerisi için /film,
dizi önerisi için /dizi yazman yeterli.", message.Chat.Name);
                        break;
                    case "selam":
                        result = $"Selam :)";
                        break;
                    case "naber":
                        result = $"İyidir, senden naber?";
                        break;
                    case "/animasyon":
                        SendDiceAsync();
                        break;
                    case "/anket":
                        SendPollAsync("Bu bir test anketidir", new string[] { "Cevap 1", "Cevap 2", "Cevap 3", "Cevap 4", "Cevap 5" }, false);
                        break;
                    case "/film":
                        SendMessageAsync("Tür seç", KeyboardButtons());
                        break;
                    case "/dizi":
                        SendMessageAsync("Tür seç", KeyboardButtons());
                        break;
                    case var _ when messageText.Replace(" ", "") == SectionEnum.TheBest.ToString().ToLower() ||
                                    messageText == SectionEnum.Trending.ToString().ToLower() ||
                                    messageText == SectionEnum.TopRated.ToString().ToLower() ||
                                    messageText == SectionEnum.Upcoming.ToString().ToLower() ||
                                    messageText == SectionEnum.Action.ToString().ToLower() ||
                                    messageText == SectionEnum.Crime.ToString().ToLower() ||
                                    messageText == SectionEnum.Comedy.ToString().ToLower() ||
                                    messageText == SectionEnum.History.ToString().ToLower() ||
                                    messageText == SectionEnum.Horror.ToString().ToLower() ||
                                    messageText == SectionEnum.Mystery.ToString().ToLower() ||
                                    messageText == SectionEnum.Romance.ToString().ToLower() ||
                                    messageText == SectionEnum.Thriller.ToString().ToLower():
                        CastInputMediaAsync(messageText.Replace(" ", ""));
                        break;
                    default:
                        result = "Üzgünüm, anlamadım.";
                        break;
                }

                if (!string.IsNullOrEmpty(result))
                    SendMessageAsync(result);
            }
        }

        async Task<string> RequestAsync(string methodName, object parameters)
        {
            string url = API_URL + this._token + "/" + methodName;
            HttpWebRequest request = null;
            WebResponse response = null;

            string responseText = await Task.Run(() =>
            {
                try
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(parameters.ToJson());

                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.Timeout = 20000;
                    request.Accept = "application/json";
                    request.ContentType = "application/json; charset=UTF-8";
                    request.ContentLength = byteArray.Length;

                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    response = request.GetResponse();
                    dataStream = response.GetResponseStream();

                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    dataStream.Close();

                    return responseFromServer;
                }
                catch (Exception e)
                {
                    throw new Exception("Error: " + e.Message);
                }
                finally
                {
                    if (response != null)
                        response.Close();
                }
                return null;
            });

            return responseText;
        }

        async Task<List<GithubMovie>> RequestGithubMovieAsync()
        {
            string url = "https://raw.githubusercontent.com/FEND16/movie-json-data/master/json/movies-in-theaters.json";
            HttpWebRequest request = null;
            WebResponse response = null;

            List<GithubMovie> responses = await Task.Run(() =>
            {
                try
                {
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    request.Timeout = 20000;
                    request.Accept = "application/json";
                    request.ContentType = "application/json; charset=UTF-8";

                    response = request.GetResponse();
                    StreamReader sr = new StreamReader(response.GetResponseStream());

                    List<GithubMovie> result = sr.ReadToEnd().FromJson<List<GithubMovie>>()
                    .OrderBy(c => Guid.NewGuid())
                    .Take(5)
                    .ToList();
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
                finally
                {
                    if (response != null)
                        response.Close();
                }
                return null;
            });

            return responses;
        }

        async Task<TmdbResult[]> RequestTMDBAPIAsync(string url)
        {
            HttpWebRequest request = null;
            WebResponse response = null;

            TmdbResult[] responses = await Task.Run(() =>
           {
               try
               {
                   request = (HttpWebRequest)WebRequest.Create(url);
                   request.Method = "GET";
                   request.Timeout = 20000;
                   request.Accept = "application/json";
                   request.ContentType = "application/json; charset=UTF-8";

                   response = request.GetResponse();
                   StreamReader sr = new StreamReader(response.GetResponseStream());

                   var result = JObject.Parse(sr.ReadToEnd())["results"].ToObject<TmdbResult[]>()
                   .Where(c => !string.IsNullOrEmpty(c.PosterImagePath) || !string.IsNullOrEmpty(c.BackdropImagePath))
                   .OrderBy(c => Guid.NewGuid())
                   .Take(5)
                   .ToArray();

                   return result;
               }
               catch (Exception e)
               {
                   Console.WriteLine("Error: " + e.Message);
               }
               finally
               {
                   if (response != null)
                       response.Close();
               }
               return null;
           });

            return responses;
        }

        /// <summary>
        /// Dizi ve film(ler) için kulanılacak.
        /// </summary>
        /// <returns></returns>
        private ReplyKeyboardMarkup KeyboardButtons()
        {
            var rmu = new ReplyKeyboardMarkup();

            rmu.Keyboard =
            new KeyboardButton[][]
            {
                    new KeyboardButton[]
                    {
                        new KeyboardButton(){ Text="The Best",CallbackData=TmdbApiUrl.Section.TheBest },
                        new KeyboardButton(){ Text="Trending", CallbackData = TmdbApiUrl.Section.Trending },
                        new KeyboardButton(){ Text="TopRated",CallbackData=TmdbApiUrl.Section.TopRated },
                        new KeyboardButton(){ Text="Upcoming",CallbackData=TmdbApiUrl.Section.Upcoming }
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton(){ Text="Action", CallbackData=TmdbApiUrl.Section.Action },
                        new KeyboardButton(){ Text="Crime", CallbackData=TmdbApiUrl.Section.Crime },
                        new KeyboardButton(){ Text="Comedy", CallbackData=TmdbApiUrl.Section.Comedy },
                        new KeyboardButton(){ Text="History",CallbackData=TmdbApiUrl.Section.History }
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton(){ Text="Horror", CallbackData=TmdbApiUrl.Section.Horror },
                        new KeyboardButton(){ Text="Mystery", CallbackData=TmdbApiUrl.Section.Mystery },
                        new KeyboardButton(){ Text="Romance", CallbackData=TmdbApiUrl.Section.Romance },
                        new KeyboardButton(){ Text="Thriller", CallbackData=TmdbApiUrl.Section.Thriller }
                    }
            };

            return rmu;
        }

        private async void CastInputMediaAsync(string sectionName)
        {
            SendChatActionAsync(Static.ChatAction.Photo);

            string url = string.Empty;

            TmdbApiUrl.SetType(TmdbTypeEnum.Movie); //or tv?

            Type type = typeof(TmdbApiUrl.Section);
            FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fi in fields)
            {
                if (fi.Name.ToLower().Equals(sectionName))
                {
                    url = fi.GetValue(null).ToString();
                    break;
                }
            }

            var tv = await RequestTMDBAPIAsync(url);
            if (tv != null)
            {
                List<InputMediaPhoto> inputMediaPhotos = tv.Select(c => new InputMediaPhoto()
                {
                    media = c.PosterImagePath ?? c.BackdropImagePath,
                    caption = string.Format("{0}", c.title)
                }).ToList();
                SendMediaPhotoGroupAsync(inputMediaPhotos);
            }
            else
                SendMessageAsync("Üzgünüm, sonuç bulamadım.");
        }
    }
}
