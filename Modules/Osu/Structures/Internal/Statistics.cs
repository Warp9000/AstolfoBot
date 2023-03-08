using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures.Internal
{
    public struct Statistics
    {
        [JsonProperty("level")]
        public Level Level { get; set; }

        [JsonProperty("global_rank")]
        public int GlobalRank { get; set; }

        [JsonProperty("global_rank_exp")]
        public int GlobalRankExp { get; set; }

        [JsonProperty("pp")]
        public double Pp { get; set; }

        [JsonProperty("pp_exp")]
        public double PpExp { get; set; }

        [JsonProperty("ranked_score")]
        public int RankedScore { get; set; }

        [JsonProperty("hit_accuracy")]
        public double HitAccuracy { get; set; }

        [JsonProperty("play_count")]
        public int PlayCount { get; set; }

        [JsonProperty("play_time")]
        public int PlayTime { get; set; }

        [JsonProperty("total_score")]
        public long TotalScore { get; set; }

        [JsonProperty("total_hits")]
        public int TotalHits { get; set; }

        [JsonProperty("maximum_combo")]
        public int MaximumCombo { get; set; }

        [JsonProperty("replays_watched_by_others")]
        public int ReplaysWatchedByOthers { get; set; }

        [JsonProperty("is_ranked")]
        public bool IsRanked { get; set; }

        [JsonProperty("grade_counts")]
        public GradeCounts GradeCounts { get; set; }

        [JsonProperty("country_rank")]
        public int CountryRank { get; set; }

        [JsonProperty("variants")]
        public List<Variant> Variants { get; set; }
    }
}