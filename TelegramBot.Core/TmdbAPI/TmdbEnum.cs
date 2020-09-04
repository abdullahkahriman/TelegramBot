using System.ComponentModel;

namespace TelegramBot.Core.TmdbAPI
{
    public enum TmdbTypeEnum
    {
        [Description("movie")]
        Movie = 1,
        [Description("tv")]
        TV = 2
    }

    public enum SectionEnum
    {
        TheBest,
        Trending,
        TopRated,
        Upcoming,
        Action,
        Crime,
        Comedy,
        History,
        Horror,
        Mystery,
        Romance,
        Thriller
    }
}
