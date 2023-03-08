using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct Kudosu
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("available")]
        public int Available { get; set; }
    }
}