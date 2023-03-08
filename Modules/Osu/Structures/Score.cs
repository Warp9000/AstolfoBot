using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures
{
    public struct Score
    {
        [JsonProperty("accuracy")]
        public double Accuracy;

        [JsonProperty("best_id")]
        public long? BestId;

        [JsonProperty("created_at")]
        public DateTime CreatedAt;

        [JsonProperty("id")]
        public object Id;

        [JsonProperty("max_combo")]
        public int MaxCombo;

        [JsonProperty("mode")]
        public string Mode;

        [JsonProperty("mode_int")]
        public int ModeInt;

        [JsonProperty("mods")]
        public List<string> Mods;

        [JsonProperty("passed")]
        public bool Passed;

        [JsonProperty("perfect")]
        public bool Perfect;

        [JsonProperty("pp")]
        public double? Pp;

        [JsonProperty("rank")]
        public string Rank;

        [JsonProperty("replay")]
        public bool Replay;

        [JsonProperty("score")]
        public int Score_;

        [JsonProperty("statistics")]
        public Internal.Statistics Statistics;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("user_id")]
        public int UserId;

        [JsonProperty("current_user_attributes")]
        public Internal.CurrentUserAttributes CurrentUserAttributes;

        [JsonProperty("beatmap")]
        public Beatmap Beatmap;

        [JsonProperty("beatmapset")]
        public Beatmapset Beatmapset;

        [JsonProperty("user")]
        public User User;
    }
}