using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct Covers
    {
        [JsonProperty("cover")]
        public string Cover;

        [JsonProperty("cover@2x")]
        public string Cover2x;

        [JsonProperty("card")]
        public string Card;

        [JsonProperty("card@2x")]
        public string Card2x;

        [JsonProperty("list")]
        public string List;

        [JsonProperty("list@2x")]
        public string List2x;

        [JsonProperty("slimcover")]
        public string Slimcover;

        [JsonProperty("slimcover@2x")]
        public string Slimcover2x;
    }
}