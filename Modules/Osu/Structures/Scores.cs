using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures
{
    public struct Scores
    {
        [JsonProperty("scores")]
        public List<Score> Scores_;
    }
}