using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TelegramBot.Core.TmdbAPI
{
    public static class TmdbApiUrl
    {
        public const string TMDB_TOKEN = "c900dfcdc02a5d54fabe53b6c69a4cc1";
        public const string TMDB_API = "https://api.themoviedb.org/3";
        public const string TMDB_IMG_URL = "https://image.tmdb.org/t/p/original";
        private static string _typeEnum = string.Empty;
        private static string _sectionEnum = string.Empty;

        public static void SetType(TmdbTypeEnum type)
        {
            _typeEnum = type.GetDescription();

            if (string.IsNullOrEmpty(_typeEnum))
                _typeEnum = TmdbTypeEnum.Movie.GetDescription();
        }

        public static class Section
        {
            public static string TheBest = TMDB_API + $"/discover/{_typeEnum}?api_key={TMDB_TOKEN}&language=tr-TR&sort_by=vote_count.desc";
            public static string Trending = TMDB_API + $"/trending/all/day?api_key={TMDB_TOKEN}&language=tr-TR";
            public static string TopRated = TMDB_API + $"/{_typeEnum}/top_rated?api_key={TMDB_TOKEN}&language=tr-TR";
            public static string Upcoming = TMDB_API + $"/{_typeEnum}/upcoming?api_key={TMDB_TOKEN}&language=tr-TR";
            public static string Action = TMDB_API + $"/discover/{_typeEnum}?api_key={TMDB_TOKEN}&language=tr-TR&sort_by=popularity.desc&with_genres=28";
            public static string Crime = TMDB_API + $"/discover/{_typeEnum}?api_key={TMDB_TOKEN}&language=tr-TR&sort_by=popularity.desc&with_genres=80";
            public static string Comedy = TMDB_API + $"/discover/{_typeEnum}?api_key={TMDB_TOKEN}&language=tr-TR&sort_by=popularity.desc&with_genres=35";
            public static string History = TMDB_API + $"/discover/{_typeEnum}?api_key={TMDB_TOKEN}&language=tr-TR&sort_by=popularity.desc&with_genres=36";
            public static string Horror = TMDB_API + $"/discover/{_typeEnum}?api_key={TMDB_TOKEN}&language=tr-TR&sort_by=popularity.desc&with_genres=27";
            public static string Mystery = TMDB_API + $"/discover/{_typeEnum}?api_key={TMDB_TOKEN}&language=tr-TR&sort_by=popularity.desc&with_genres=9648";
            public static string Romance = TMDB_API + $"/discover/{_typeEnum}?api_key={TMDB_TOKEN}&language=tr-TR&sort_by=popularity.desc&with_genres=10749";
            public static string Thriller = TMDB_API + $"/discover/{_typeEnum}?api_key={TMDB_TOKEN}&language=tr-TR&sort_by=popularity.desc&with_genres=53";
        }
    }
}
