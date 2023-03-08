using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct Variant
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("variant")]
        public string VariantStr { get; set; }

        [JsonProperty("country_rank")]
        public int CountryRank { get; set; }

        [JsonProperty("global_rank")]
        public int GlobalRank { get; set; }

        [JsonProperty("pp")]
        public double Pp { get; set; }
    }
}