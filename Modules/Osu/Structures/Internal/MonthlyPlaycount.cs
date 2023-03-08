using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct MonthlyPlaycount
    {
        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}