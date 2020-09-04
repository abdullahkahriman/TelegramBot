using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Core.DTO
{
    public class Log : Table
    {
        [Required]
        public string Message { get; set; }
    }
}