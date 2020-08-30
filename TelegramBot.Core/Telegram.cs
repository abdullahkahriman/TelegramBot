using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Core.Model;
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

        public async void SendMessageAsync(string message)
        {
            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(this._chatID))
            {
                var obj = new
                {
                    chat_id = this._chatID,
                    text = message
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
                        SendChatActionAsync(Static.ChatAction.Photo);

                        List<GithubMovie> movies = await RequestGithubMovieAsync();
                        if (movies != null)
                        {
                            List<InputMediaPhoto> inputMediaPhotos = movies.Select(c => new InputMediaPhoto()
                            {
                                media = c.posterurl,
                                caption = string.Format("Title: {0}\nYear: {1}\nActors: {2}", c.title, c.year, string.Join(", ", c.actors))
                            }).ToList();
                            SendMediaPhotoGroupAsync(inputMediaPhotos);
                        }
                        else
                            result = "Üzgünüm, film bulamadım.";
                        break;
                    case "/dizi":
                        result = "Üzgünüm, şu an bu kısım aktif değil.";
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
                    Console.WriteLine("Error: " + e.Message);
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
    }
}
