using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct RelatedUser
    {
        [JsonProperty("avatar_url")]
        public string AvatarUrl;

        [JsonProperty("country_code")]
        public string CountryCode;

        [JsonProperty("default_group")]
        public string DefaultGroup;

        [JsonProperty("id")]
        public int Id;

        [JsonProperty("is_active")]
        public bool IsActive;

        [JsonProperty("is_bot")]
        public bool IsBot;

        [JsonProperty("is_deleted")]
        public bool IsDeleted;

        [JsonProperty("is_online")]
        public bool IsOnline;

        [JsonProperty("is_supporter")]
        public bool IsSupporter;

        [JsonProperty("last_visit")]
        public DateTime LastVisit;

        [JsonProperty("pm_friends_only")]
        public bool PmFriendsOnly;

        [JsonProperty("profile_colour")]
        public object ProfileColour;

        [JsonProperty("username")]
        public string Username;
    }
}