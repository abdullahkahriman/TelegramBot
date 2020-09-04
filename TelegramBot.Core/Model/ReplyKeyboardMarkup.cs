using System.Collections.Generic;

namespace TelegramBot.Core.Model
{
    public class ReplyKeyboardMarkup
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "keyboard")]
        public IEnumerable<IEnumerable<KeyboardButton>> Keyboard { get; set; }
    }
}
