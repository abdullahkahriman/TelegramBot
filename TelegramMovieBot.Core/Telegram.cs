using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TelegramMovieBot.Core
{
    public class Telegram
    {
        private const string API_URL = "https://api.telegram.org/bot";
        private string _token;
        private string _chatID;

        public Telegram(string token)
        {
            this._token = token;
        }

        public async void SetWebhookAsync(string url)
        {
            var obj = new
            {
                url = url
            };

            await RequestAsync("setWebhook", obj);
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

        public Message GetChat(Stream stream)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    string response = streamReader.ReadToEnd();
                    JObject jObject = JObject.Parse(response);
                    Message message = jObject.SelectToken("message").ToObject<Message>();
                    this._chatID = message.Chat.ID;
                    return message;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Explode(Message message)
        {
            string result = string.Empty;

            if (message != null)
            {
                string messageText = message.Text?.ToLower();
                switch (messageText)
                {
                    case "selam":
                        result = $"Selam :)";
                        break;
                    case "naber":
                        result = $"İyidir, senden naber?";
                        break;
                    default:
                        result = "Üzgünüm, anlamadım.";
                        break;
                }

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

                    //WebResponse response = request.GetResponse();
                    //Stream responseStream = response.GetResponseStream();
                    //return new StreamReader(responseStream).ReadToEnd();
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
    }
}
