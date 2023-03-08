using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct Availability
    {
        [JsonProperty("download_disabled")]
        public bool DownloadDisabled;

        [JsonProperty("more_information")]
        public object MoreInformation;
    }
}