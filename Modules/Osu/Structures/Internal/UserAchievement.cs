using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct UserAchievement
    {
        [JsonProperty("achieved_at")]
        public DateTime AchievedAt { get; set; }

        [JsonProperty("achievement_id")]
        public int AchievementId { get; set; }
    }
}