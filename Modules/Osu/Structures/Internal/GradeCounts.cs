using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct GradeCounts
    {
        [JsonProperty("ss")]
        public int SS { get; set; }

        [JsonProperty("ssh")]
        public int SSH { get; set; }

        [JsonProperty("s")]
        public int S { get; set; }

        [JsonProperty("sh")]
        public int SH { get; set; }

        [JsonProperty("a")]
        public int A { get; set; }
    }
}