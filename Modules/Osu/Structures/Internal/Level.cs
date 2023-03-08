using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct Level
    {
        [JsonProperty("current")]
        public int Current { get; set; }

        [JsonProperty("progress")]
        public int Progress { get; set; }
    }
}