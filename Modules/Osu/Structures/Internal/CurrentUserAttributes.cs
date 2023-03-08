using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct CurrentUserAttributes
    {
        [JsonProperty("pin")]
        public object Pin;
    }
}