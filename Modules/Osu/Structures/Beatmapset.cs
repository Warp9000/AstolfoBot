using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures
{
    public struct Beatmapset
    {
        [JsonProperty("artist")]
        public string Artist;

        [JsonProperty("artist_unicode")]
        public string ArtistUnicode;

        [JsonProperty("covers")]
        public Internal.Covers Covers;

        [JsonProperty("creator")]
        public string Creator;

        [JsonProperty("favourite_count")]
        public int FavouriteCount;

        [JsonProperty("hype")]
        public object Hype;

        [JsonProperty("id")]
        public int Id;

        [JsonProperty("nsfw")]
        public bool Nsfw;

        [JsonProperty("offset")]
        public int Offset;

        [JsonProperty("play_count")]
        public int PlayCount;

        [JsonProperty("preview_url")]
        public string PreviewUrl;

        [JsonProperty("source")]
        public string Source;

        [JsonProperty("spotlight")]
        public bool Spotlight;

        [JsonProperty("status")]
        public string Status;

        [JsonProperty("title")]
        public string Title;

        [JsonProperty("title_unicode")]
        public string TitleUnicode;

        [JsonProperty("track_id")]
        public object TrackId;

        [JsonProperty("user_id")]
        public int UserId;

        [JsonProperty("video")]
        public bool Video;

        [JsonProperty("availability")]
        public Internal.Availability Availability;

        [JsonProperty("bpm")]
        public double Bpm;

        [JsonProperty("can_be_hyped")]
        public bool CanBeHyped;

        [JsonProperty("discussion_enabled")]
        public bool DiscussionEnabled;

        [JsonProperty("discussion_locked")]
        public bool DiscussionLocked;

        [JsonProperty("is_scoreable")]
        public bool IsScoreable;

        [JsonProperty("last_updated")]
        public DateTime LastUpdated;

        [JsonProperty("legacy_thread_url")]
        public string LegacyThreadUrl;

        [JsonProperty("nominations_summary")]
        public Internal.NominationsSummary NominationsSummary;

        [JsonProperty("ranked")]
        public int Ranked;

        [JsonProperty("ranked_date")]
        public DateTime RankedDate;

        [JsonProperty("storyboard")]
        public bool Storyboard;

        [JsonProperty("submitted_date")]
        public DateTime SubmittedDate;

        [JsonProperty("tags")]
        public string Tags;

        [JsonProperty("beatmaps")]
        public List<Beatmap> Beatmaps;

        [JsonProperty("converts")]
        public List<Beatmap> Converts;

        [JsonProperty("current_nominations")]
        public List<object> CurrentNominations;

        [JsonProperty("description.description")]
        public string Description;

        [JsonProperty("genre")]
        public Internal.Genre Genre;

        [JsonProperty("language")]
        public Internal.Language Language;

        [JsonProperty("ratings")]
        public List<int> Ratings;

        [JsonProperty("recent_favourites")]
        public List<Internal.RecentFavourite> RecentFavourites;

        [JsonProperty("related_users")]
        public List<Internal.RelatedUser> RelatedUsers;

        [JsonProperty("user")]
        public User User;
    }
}