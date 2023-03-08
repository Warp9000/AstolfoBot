using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures
{
    public struct Beatmapsets
    {
        [JsonProperty("error")]
        public string Error;

        [JsonProperty("beatmapsets")]
        public List<Beatmapset> Sets;

        // [JsonProperty("search")]
        // public Search Search;

        [JsonProperty("recommended_difficulty")]
        public object RecommendedDifficulty;

        [JsonProperty("total")]
        public int Total;

        [JsonProperty("cursor")]
        public object Cursor;

        [JsonProperty("cursor_string")]
        public object CursorString;
    }
}