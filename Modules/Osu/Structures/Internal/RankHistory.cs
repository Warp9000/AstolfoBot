using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct RankHistory
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("data")]
        public List<int> Data { get; set; }
    }
}