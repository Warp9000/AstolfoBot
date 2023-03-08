using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct Page
    {
        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("raw")]
        public string Raw { get; set; }
    }
}