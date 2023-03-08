using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu.Api
{
    public class Structures_
    {
        // ignore null warnings
#pragma warning disable CS8618
        public enum GameMode
        {
            osu = 0,
            taiko = 1,
            fruits = 2,
            mania = 3
        }

        public class Root
        {
            public string error { get; set; }
        }

        public class User : Root
        {
            [JsonProperty("avatar_url")]
            public string AvatarUrl { get; set; }

            [JsonProperty("country_code")]
            public string CountryCode { get; set; }

            [JsonProperty("default_group")]
            public string DefaultGroup { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("is_active")]
            public bool IsActive { get; set; }

            [JsonProperty("is_bot")]
            public bool IsBot { get; set; }

            [JsonProperty("is_deleted")]
            public bool IsDeleted { get; set; }

            [JsonProperty("is_online")]
            public bool IsOnline { get; set; }

            [JsonProperty("is_supporter")]
            public bool IsSupporter { get; set; }

            [JsonProperty("last_visit")]
            public DateTime LastVisit { get; set; }

            [JsonProperty("pm_friends_only")]
            public bool PmFriendsOnly { get; set; }

            [JsonProperty("profile_colour")]
            public object ProfileColour { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("cover_url")]
            public string CoverUrl { get; set; }

            [JsonProperty("discord")]
            public string Discord { get; set; }

            [JsonProperty("has_supported")]
            public bool HasSupported { get; set; }

            [JsonProperty("interests")]
            public object Interests { get; set; }

            [JsonProperty("join_date")]
            public DateTime JoinDate { get; set; }

            [JsonProperty("kudosu")]
            public Sub.Kudosu Kudosu { get; set; }

            [JsonProperty("location")]
            public object Location { get; set; }

            [JsonProperty("max_blocks")]
            public int MaxBlocks { get; set; }

            [JsonProperty("max_friends")]
            public int MaxFriends { get; set; }

            [JsonProperty("occupation")]
            public object Occupation { get; set; }

            [JsonProperty("playmode")]
            public GameMode Playmode { get; set; }

            [JsonProperty("playstyle")]
            public List<string> Playstyle { get; set; }

            [JsonProperty("post_count")]
            public int PostCount { get; set; }

            [JsonProperty("profile_order")]
            public List<string> ProfileOrder { get; set; }

            [JsonProperty("title")]
            public object Title { get; set; }

            [JsonProperty("title_url")]
            public object TitleUrl { get; set; }

            [JsonProperty("twitter")]
            public object Twitter { get; set; }

            [JsonProperty("website")]
            public object Website { get; set; }

            [JsonProperty("country")]
            public Sub.Country Country { get; set; }

            [JsonProperty("cover")]
            public Sub.Cover Cover { get; set; }

            [JsonProperty("account_history")]
            public List<object> AccountHistory { get; set; }

            [JsonProperty("active_tournament_banner")]
            public object ActiveTournamentBanner { get; set; }

            [JsonProperty("badges")]
            public List<object> Badges { get; set; }

            [JsonProperty("beatmap_playcounts_count")]
            public int BeatmapPlaycountsCount { get; set; }

            [JsonProperty("comments_count")]
            public int CommentsCount { get; set; }

            [JsonProperty("favourite_beatmapset_count")]
            public int FavouriteBeatmapsetCount { get; set; }

            [JsonProperty("follower_count")]
            public int FollowerCount { get; set; }

            [JsonProperty("graveyard_beatmapset_count")]
            public int GraveyardBeatmapsetCount { get; set; }

            [JsonProperty("groups")]
            public List<object> Groups { get; set; }

            [JsonProperty("guest_beatmapset_count")]
            public int GuestBeatmapsetCount { get; set; }

            [JsonProperty("loved_beatmapset_count")]
            public int LovedBeatmapsetCount { get; set; }

            [JsonProperty("mapping_follower_count")]
            public int MappingFollowerCount { get; set; }

            [JsonProperty("monthly_playcounts")]
            public List<Sub.MonthlyPlaycount> MonthlyPlaycounts { get; set; }

            [JsonProperty("nominated_beatmapset_count")]
            public int NominatedBeatmapsetCount { get; set; }

            [JsonProperty("page")]
            public Sub.Page Page { get; set; }

            [JsonProperty("pending_beatmapset_count")]
            public int PendingBeatmapsetCount { get; set; }

            [JsonProperty("previous_usernames")]
            public List<object> PreviousUsernames { get; set; }

            [JsonProperty("rank_highest")]
            public Sub.RankHighest RankHighest { get; set; }

            [JsonProperty("ranked_beatmapset_count")]
            public int RankedBeatmapsetCount { get; set; }

            [JsonProperty("replays_watched_counts")]
            public List<object> ReplaysWatchedCounts { get; set; }

            [JsonProperty("scores_best_count")]
            public int ScoresBestCount { get; set; }

            [JsonProperty("scores_first_count")]
            public int ScoresFirstCount { get; set; }

            [JsonProperty("scores_pinned_count")]
            public int ScoresPinnedCount { get; set; }

            [JsonProperty("scores_recent_count")]
            public int ScoresRecentCount { get; set; }

            [JsonProperty("statistics")]
            public Sub.Statistics Statistics { get; set; }

            [JsonProperty("support_level")]
            public int SupportLevel { get; set; }

            [JsonProperty("user_achievements")]
            public List<Sub.UserAchievement> UserAchievements { get; set; }

            [JsonProperty("rank_history")]
            public Sub.RankHistory RankHistory { get; set; }

            [JsonProperty("ranked_and_approved_beatmapset_count")]
            public int RankedAndApprovedBeatmapsetCount { get; set; }

            [JsonProperty("unranked_beatmapset_count")]
            public int UnrankedBeatmapsetCount { get; set; }
        }

        public class Score : Root
        {
            public double accuracy { get; set; }
            public object best_id { get; set; }
            public DateTime created_at { get; set; }
            public object id { get; set; }
            public int max_combo { get; set; }
            public string mode { get; set; }
            public int mode_int { get; set; }
            public List<object> mods { get; set; }
            public bool passed { get; set; }
            public bool perfect { get; set; }
            public object pp { get; set; }
            public string rank { get; set; }
            public bool replay { get; set; }
            public int score { get; set; }
            public Sub.Statistics statistics { get; set; }
            public string type { get; set; }
            public int user_id { get; set; }
            public Sub.CurrentUserAttributes current_user_attributes { get; set; }
            public Sub.Beatmap beatmap { get; set; }
            public Sub.Beatmapset beatmapset { get; set; }
            public Sub.User user { get; set; }
        }

        public class Beatmapset : Root
        {
            public string artist { get; set; }
            public string artist_unicode { get; set; }
            public Sub.Covers covers { get; set; }
            public string creator { get; set; }
            public int favourite_count { get; set; }
            public object hype { get; set; }
            public int id { get; set; }
            public bool nsfw { get; set; }
            public int offset { get; set; }
            public int play_count { get; set; }
            public string preview_url { get; set; }
            public string source { get; set; }
            public bool spotlight { get; set; }
            public string status { get; set; }
            public string title { get; set; }
            public string title_unicode { get; set; }
            public object track_id { get; set; }
            public int user_id { get; set; }
            public bool video { get; set; }
            public Sub.Availability availability { get; set; }
            public double bpm { get; set; }
            public bool can_be_hyped { get; set; }
            public bool discussion_enabled { get; set; }
            public bool discussion_locked { get; set; }
            public bool is_scoreable { get; set; }
            public DateTime last_updated { get; set; }
            public string legacy_thread_url { get; set; }
            public Sub.NominationsSummary nominations_summary { get; set; }
            public int ranked { get; set; }
            public DateTime ranked_date { get; set; }
            public bool storyboard { get; set; }
            public DateTime submitted_date { get; set; }
            public string tags { get; set; }
            public List<Sub.Beatmap> beatmaps { get; set; }
            public List<Sub.Convert> converts { get; set; }
            public List<object> current_nominations { get; set; }
            public Sub.Description description { get; set; }
            public Sub.Genre genre { get; set; }
            public Sub.Language language { get; set; }
            public List<int> ratings { get; set; }
            public List<Sub.RecentFavourite> recent_favourites { get; set; }
            public List<Sub.RelatedUser> related_users { get; set; }
            public Sub.User user { get; set; }
        }

        public class Beatmapsets : Root
        {
            public List<Sub.Beatmapset> beatmapsets { get; set; }
            public Sub.Search search { get; set; }
            public object recommended_difficulty { get; set; }
            public int total { get; set; }
            public object cursor { get; set; }
            public object cursor_string { get; set; }
        }

        public class Beatmap : Root
        {
            public int beatmapset_id { get; set; }
            public double difficulty_rating { get; set; }
            public int id { get; set; }
            public string mode { get; set; }
            public string status { get; set; }
            public int total_length { get; set; }
            public int user_id { get; set; }
            public string version { get; set; }
            public double accuracy { get; set; }
            public double ar { get; set; }
            public double bpm { get; set; }
            public bool convert { get; set; }
            public int count_circles { get; set; }
            public int count_sliders { get; set; }
            public int count_spinners { get; set; }
            public double cs { get; set; }
            public object deleted_at { get; set; }
            public float drain { get; set; }
            public int hit_length { get; set; }
            public bool is_scoreable { get; set; }
            public DateTime last_updated { get; set; }
            public int mode_int { get; set; }
            public int passcount { get; set; }
            public int playcount { get; set; }
            public int ranked { get; set; }
            public string url { get; set; }
            public string checksum { get; set; }
            public Sub.Beatmapset beatmapset { get; set; }
            public Sub.Failtimes failtimes { get; set; }
            public int max_combo { get; set; }
        }

        public class Scores : Root
        {
            public List<Sub.Score> scores { get; set; }
        }

        public class Sub
        {
            public class Country
            {
                [JsonProperty("code")]
                public string Code { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }
            }

            public class Cover
            {
                [JsonProperty("custom_url")]
                public string CustomUrl { get; set; }

                [JsonProperty("url")]
                public string Url { get; set; }

                [JsonProperty("id")]
                public object Id { get; set; }
            }

            public class GradeCounts
            {
                [JsonProperty("ss")]
                public int SS { get; set; }

                [JsonProperty("ssh")]
                public int SSH { get; set; }

                [JsonProperty("s")]
                public int S { get; set; }

                [JsonProperty("sh")]
                public int SH { get; set; }

                [JsonProperty("a")]
                public int A { get; set; }
            }

            public class Kudosu
            {
                [JsonProperty("total")]
                public int Total { get; set; }

                [JsonProperty("available")]
                public int Available { get; set; }
            }

            public class Level
            {
                [JsonProperty("current")]
                public int Current { get; set; }

                [JsonProperty("progress")]
                public int Progress { get; set; }
            }

            public class MonthlyPlaycount
            {
                [JsonProperty("start_date")]
                public string StartDate { get; set; }

                [JsonProperty("count")]
                public int Count { get; set; }
            }

            public class Page
            {
                [JsonProperty("html")]
                public string Html { get; set; }

                [JsonProperty("raw")]
                public string Raw { get; set; }
            }

            public class Rank
            {
                [JsonProperty("country")]
                public int Country { get; set; }
            }

            public class RankHighest
            {
                [JsonProperty("rank")]
                public int Rank { get; set; }

                [JsonProperty("updated_at")]
                public DateTime UpdatedAt { get; set; }
            }

            public class RankHistory
            {
                [JsonProperty("mode")]
                public string Mode { get; set; }

                [JsonProperty("data")]
                public List<int> Data { get; set; }
            }

            public class HitStatistics
            {
                [JsonProperty("count_geki")]
                public int CountGeki { get; set; }
                [JsonProperty("count_300")]
                public int Count300 { get; set; }
                [JsonProperty("count_katu")]
                public int CountKatu { get; set; }
                [JsonProperty("count_100")]
                public int Count100 { get; set; }
                [JsonProperty("count_50")]
                public int Count50 { get; set; }
                [JsonProperty("count_miss")]
                public int CountMiss { get; set; }
            }

            public class Statistics
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

                [JsonProperty("rank")]
                public Rank Rank { get; set; }

                [JsonProperty("variants")]
                public List<Variant> Variants { get; set; }
            }

            public class UserAchievement
            {
                [JsonProperty("achieved_at")]
                public DateTime AchievedAt { get; set; }

                [JsonProperty("achievement_id")]
                public int AchievementId { get; set; }
            }

            public class Variant
            {
                [JsonProperty("mode")]
                public string Mode { get; set; }

                [JsonProperty("variant")]
                public string VariantStr { get; set; }

                [JsonProperty("country_rank")]
                public int CountryRank { get; set; }

                [JsonProperty("global_rank")]
                public int GlobalRank { get; set; }

                [JsonProperty("pp")]
                public double Pp { get; set; }
            }


            public class Beatmap
            {
                public int beatmapset_id { get; set; }
                public double difficulty_rating { get; set; }
                public int id { get; set; }
                public string mode { get; set; }
                public string status { get; set; }
                public int total_length { get; set; }
                public int user_id { get; set; }
                public string version { get; set; }
                public double accuracy { get; set; }
                public double ar { get; set; }
                public int bpm { get; set; }
                public bool convert { get; set; }
                public int count_circles { get; set; }
                public int count_sliders { get; set; }
                public int count_spinners { get; set; }
                public double cs { get; set; }
                public object deleted_at { get; set; }
                public double drain { get; set; }
                public int hit_length { get; set; }
                public bool is_scoreable { get; set; }
                public DateTime last_updated { get; set; }
                public int mode_int { get; set; }
                public int passcount { get; set; }
                public int playcount { get; set; }
                public int ranked { get; set; }
                public string url { get; set; }
                public string checksum { get; set; }
            }

            public class Beatmapset
            {
                public string artist { get; set; }
                public string artist_unicode { get; set; }
                public Covers covers { get; set; }
                public string creator { get; set; }
                public int favourite_count { get; set; }
                public object hype { get; set; }
                public int id { get; set; }
                public bool nsfw { get; set; }
                public int offset { get; set; }
                public int play_count { get; set; }
                public string preview_url { get; set; }
                public string source { get; set; }
                public bool spotlight { get; set; }
                public string status { get; set; }
                public string title { get; set; }
                public string title_unicode { get; set; }
                public int? track_id { get; set; }
                public int user_id { get; set; }
                public bool video { get; set; }
            }

            public class Covers
            {
                public string cover { get; set; }

                [JsonProperty("cover@2x")]
                public string cover2x { get; set; }
                public string card { get; set; }

                [JsonProperty("card@2x")]
                public string card2x { get; set; }
                public string list { get; set; }

                [JsonProperty("list@2x")]
                public string list2x { get; set; }
                public string slimcover { get; set; }

                [JsonProperty("slimcover@2x")]
                public string slimcover2x { get; set; }
            }

            public class CurrentUserAttributes
            {
                public object pin { get; set; }
            }

            public class Root
            {
                public double accuracy { get; set; }
                public object best_id { get; set; }
                public DateTime created_at { get; set; }
                public object id { get; set; }
                public int max_combo { get; set; }
                public string mode { get; set; }
                public int mode_int { get; set; }
                public List<object> mods { get; set; }
                public bool passed { get; set; }
                public bool perfect { get; set; }
                public object pp { get; set; }
                public string rank { get; set; }
                public bool replay { get; set; }
                public int score { get; set; }
                public Statistics statistics { get; set; }
                public string type { get; set; }
                public int user_id { get; set; }
                public CurrentUserAttributes current_user_attributes { get; set; }
                public Beatmap beatmap { get; set; }
                public Beatmapset beatmapset { get; set; }
                public User user { get; set; }
            }

            public class User
            {
                public string avatar_url { get; set; }
                public string country_code { get; set; }
                public string default_group { get; set; }
                public int id { get; set; }
                public bool is_active { get; set; }
                public bool is_bot { get; set; }
                public bool is_deleted { get; set; }
                public bool is_online { get; set; }
                public bool is_supporter { get; set; }
                public DateTime last_visit { get; set; }
                public bool pm_friends_only { get; set; }
                public object profile_colour { get; set; }
                public string username { get; set; }
            }


            public class Availability
            {
                public bool download_disabled { get; set; }
                public object more_information { get; set; }
            }

            public class Convert
            {
                public int beatmapset_id { get; set; }
                public double difficulty_rating { get; set; }
                public int id { get; set; }
                public string mode { get; set; }
                public string status { get; set; }
                public int total_length { get; set; }
                public int user_id { get; set; }
                public string version { get; set; }
                public int accuracy { get; set; }
                public int ar { get; set; }
                public double bpm { get; set; }
                public bool convert { get; set; }
                public int count_circles { get; set; }
                public int count_sliders { get; set; }
                public int count_spinners { get; set; }
                public int cs { get; set; }
                public object deleted_at { get; set; }
                public int drain { get; set; }
                public int hit_length { get; set; }
                public bool is_scoreable { get; set; }
                public DateTime last_updated { get; set; }
                public int mode_int { get; set; }
                public int passcount { get; set; }
                public int playcount { get; set; }
                public int ranked { get; set; }
                public string url { get; set; }
                public string checksum { get; set; }
                public Failtimes failtimes { get; set; }
            }

            public class Description
            {
                public string description { get; set; }
            }

            public class Failtimes
            {
                public List<int> fail { get; set; }
                public List<int> exit { get; set; }
            }

            public class Genre
            {
                public int id { get; set; }
                public string name { get; set; }
            }

            public class Language
            {
                public int id { get; set; }
                public string name { get; set; }
            }

            public class NominationsSummary
            {
                public int current { get; set; }
                public int required { get; set; }
            }

            public class RecentFavourite
            {
                public string avatar_url { get; set; }
                public string country_code { get; set; }
                public string default_group { get; set; }
                public int id { get; set; }
                public bool is_active { get; set; }
                public bool is_bot { get; set; }
                public bool is_deleted { get; set; }
                public bool is_online { get; set; }
                public bool is_supporter { get; set; }
                public DateTime? last_visit { get; set; }
                public bool pm_friends_only { get; set; }
                public object profile_colour { get; set; }
                public string username { get; set; }
            }

            public class RelatedUser
            {
                public string avatar_url { get; set; }
                public string country_code { get; set; }
                public string default_group { get; set; }
                public int id { get; set; }
                public bool is_active { get; set; }
                public bool is_bot { get; set; }
                public bool is_deleted { get; set; }
                public bool is_online { get; set; }
                public bool is_supporter { get; set; }
                public DateTime last_visit { get; set; }
                public bool pm_friends_only { get; set; }
                public object profile_colour { get; set; }
                public string username { get; set; }
            }

            public class Beatmapsets
            {
                public List<Beatmapset> beatmapsets { get; set; }
                public Search search { get; set; }
                public object recommended_difficulty { get; set; }
                public object error { get; set; }
                public int total { get; set; }
                public object cursor { get; set; }
                public object cursor_string { get; set; }
            }

            public class Search
            {
                public string sort { get; set; }
            }

            public class Score
            {
                public double accuracy { get; set; }
                public int best_id { get; set; }
                public DateTime created_at { get; set; }
                public int id { get; set; }
                public int max_combo { get; set; }
                public string mode { get; set; }
                public int mode_int { get; set; }
                public List<string> mods { get; set; }
                public bool passed { get; set; }
                public bool perfect { get; set; }
                public double pp { get; set; }
                public string rank { get; set; }
                public bool replay { get; set; }
                public int score { get; set; }
                public Statistics statistics { get; set; }
                public string type { get; set; }
                public int user_id { get; set; }
                public CurrentUserAttributes current_user_attributes { get; set; }
                public User user { get; set; }
            }

        }
        // enable null warnings again
#pragma warning restore CS8618
    }
}