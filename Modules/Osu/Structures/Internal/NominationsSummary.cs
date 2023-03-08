using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct NominationsSummary
    {
        [JsonProperty("current")]
        public int Current;

        [JsonProperty("required")]
        public int Required;
    }
}