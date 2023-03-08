using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct Cover
    {
        [JsonProperty("custom_url")]
        public string CustomUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("id")]
        public object Id { get; set; }
    }
}