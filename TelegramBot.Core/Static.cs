using System.ComponentModel;

namespace TelegramBot.Core
{
    public static class Static
    {
        public const string TOKEN = "1335790071:AAHjvauoB_TTwxaYBtBld_XA7znIvL_Iyis";

        public enum ChatAction
        {
            /// <summary>
            /// For text messages
            /// </summary>
            [Description("typing")]
            Typing = 1,
            /// <summary>
            /// For photos
            /// </summary>
            [Description("upload_photo")]
            Photo = 2,
            /// <summary>
            /// For video
            /// </summary>
            [Description("record_video")]
            RecordVideo = 3,
            /// <summary>
            /// For video
            /// </summary>
            [Description("upload_video")]
            UploadVideo = 4,
            /// <summary>
            /// For audio files
            /// </summary>
            [Description("record_audio")]
            RecordAudio = 5,
            /// <summary>
            /// For audio files
            /// </summary>
            [Description("upload_audio")]
            UploadAudio = 6,
            /// <summary>
            /// For general files
            /// </summary>
            [Description("upload_document")]
            UploadDocument = 7,
            /// <summary>
            /// For location data
            /// </summary>
            [Description("find_location")]
            FindLocation = 8,
            /// <summary>
            /// For video notes
            /// </summary>
            [Description("record_video_note")]
            RecordVideoNote = 9,
            /// <summary>
            /// For video notes
            /// </summary>
            [Description("upload_video_note")]
            UploadVideoNote = 10
        }
    }
}