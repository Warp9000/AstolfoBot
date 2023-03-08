using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct Failtimes
    {
        [JsonProperty("fail")]
        public List<int> Fail;

        [JsonProperty("exit")]
        public List<int> Exit;
    }
}