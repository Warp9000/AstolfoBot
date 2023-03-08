using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Structures
{
    public struct User
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

        [JsonProperty("cover_url")]
        public string CoverUrl;

        [JsonProperty("discord")]
        public string Discord;

        [JsonProperty("has_supported")]
        public bool HasSupported;

        [JsonProperty("interests")]
        public object Interests;

        [JsonProperty("join_date")]
        public DateTime JoinDate;

        [JsonProperty("kudosu")]
        public Internal.Kudosu Kudosu;

        [JsonProperty("location")]
        public object Location;

        [JsonProperty("max_blocks")]
        public int MaxBlocks;

        [JsonProperty("max_friends")]
        public int MaxFriends;

        [JsonProperty("occupation")]
        public object Occupation;

        [JsonProperty("playmode")]
        public GameMode Playmode;

        [JsonProperty("playstyle")]
        public List<string> Playstyle;

        [JsonProperty("post_count")]
        public int PostCount;

        [JsonProperty("profile_order")]
        public List<string> ProfileOrder;

        [JsonProperty("title")]
        public object Title;

        [JsonProperty("title_url")]
        public object TitleUrl;

        [JsonProperty("twitter")]
        public object Twitter;

        [JsonProperty("website")]
        public object Website;

        [JsonProperty("country")]
        public Internal.Country Country;

        [JsonProperty("cover")]
        public Internal.Cover Cover;

        [JsonProperty("account_history")]
        public List<object> AccountHistory;

        [JsonProperty("active_tournament_banner")]
        public object ActiveTournamentBanner;

        [JsonProperty("badges")]
        public List<object> Badges;

        [JsonProperty("beatmap_playcounts_count")]
        public int BeatmapPlaycountsCount;

        [JsonProperty("comments_count")]
        public int CommentsCount;

        [JsonProperty("favourite_beatmapset_count")]
        public int FavouriteBeatmapsetCount;

        [JsonProperty("follower_count")]
        public int FollowerCount;

        [JsonProperty("graveyard_beatmapset_count")]
        public int GraveyardBeatmapsetCount;

        [JsonProperty("groups")]
        public List<object> Groups;

        [JsonProperty("guest_beatmapset_count")]
        public int GuestBeatmapsetCount;

        [JsonProperty("loved_beatmapset_count")]
        public int LovedBeatmapsetCount;

        [JsonProperty("mapping_follower_count")]
        public int MappingFollowerCount;

        [JsonProperty("monthly_playcounts")]
        public List<Internal.MonthlyPlaycount> MonthlyPlaycounts;

        [JsonProperty("nominated_beatmapset_count")]
        public int NominatedBeatmapsetCount;

        [JsonProperty("page")]
        public Internal.Page Page;

        [JsonProperty("pending_beatmapset_count")]
        public int PendingBeatmapsetCount;

        [JsonProperty("previous_usernames")]
        public List<object> PreviousUsernames;

        [JsonProperty("rank_highest")]
        public Internal.RankHighest RankHighest;

        [JsonProperty("ranked_beatmapset_count")]
        public int RankedBeatmapsetCount;

        [JsonProperty("replays_watched_counts")]
        public List<object> ReplaysWatchedCounts;

        [JsonProperty("scores_best_count")]
        public int ScoresBestCount;

        [JsonProperty("scores_first_count")]
        public int ScoresFirstCount;

        [JsonProperty("scores_pinned_count")]
        public int ScoresPinnedCount;

        [JsonProperty("scores_recent_count")]
        public int ScoresRecentCount;

        [JsonProperty("statistics")]
        public Internal.Statistics Statistics;

        [JsonProperty("support_level")]
        public int SupportLevel;

        [JsonProperty("user_achievements")]
        public List<Internal.UserAchievement> UserAchievements;

        [JsonProperty("rank_history")]
        public Internal.RankHistory RankHistory;

        [JsonProperty("ranked_and_approved_beatmapset_count")]
        public int RankedAndApprovedBeatmapsetCount;

        [JsonProperty("unranked_beatmapset_count")]
        public int UnrankedBeatmapsetCount;
    }
}