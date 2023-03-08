using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures
{
    public struct Beatmap
    {
        [JsonProperty("beatmapset_id")]
        public int BeatmapsetId;

        [JsonProperty("difficulty_rating")]
        public double DifficultyRating;

        [JsonProperty("id")]
        public int Id;

        [JsonProperty("mode")]
        public string Mode;

        [JsonProperty("status")]
        public string Status;

        [JsonProperty("total_length")]
        public int TotalLength;

        [JsonProperty("user_id")]
        public int UserId;

        [JsonProperty("version")]
        public string Version;

        [JsonProperty("accuracy")]
        public double Accuracy;

        [JsonProperty("ar")]
        public double Ar;

        [JsonProperty("bpm")]
        public double Bpm;

        [JsonProperty("convert")]
        public bool Convert;

        [JsonProperty("count_circles")]
        public int CountCircles;

        [JsonProperty("count_sliders")]
        public int CountSliders;

        [JsonProperty("count_spinners")]
        public int CountSpinners;

        [JsonProperty("cs")]
        public double Cs;

        [JsonProperty("deleted_at")]
        public object DeletedAt;

        [JsonProperty("drain")]
        public int Drain;

        [JsonProperty("hit_length")]
        public int HitLength;

        [JsonProperty("is_scoreable")]
        public bool IsScoreable;

        [JsonProperty("last_updated")]
        public DateTime LastUpdated;

        [JsonProperty("mode_int")]
        public int ModeInt;

        [JsonProperty("passcount")]
        public int Passcount;

        [JsonProperty("playcount")]
        public int Playcount;

        [JsonProperty("ranked")]
        public int Ranked;

        [JsonProperty("url")]
        public string Url;

        [JsonProperty("checksum")]
        public string Checksum;

        [JsonProperty("beatmapset")]
        public Beatmapset Beatmapset;

        [JsonProperty("failtimes")]
        public Internal.Failtimes Failtimes;

        [JsonProperty("max_combo")]
        public int MaxCombo;
    }
}