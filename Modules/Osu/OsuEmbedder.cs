using Discord;

namespace AstolfoBot.Modules.Osu.Api
{
    public static class OsuEmbedder
    {
        public static EmbedBuilder Embed(this Structures.User user)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(user.Username);
            embed.WithThumbnailUrl(user.AvatarUrl);
            string desc = "";

            desc += $"**Rank:** #{user.Statistics.GlobalRank:N0} (#{-1:N0} in {user.CountryCode})" +
                    $"#{-1:N0} peak {new TimestampTag(new DateTimeOffset(ticks: 0, new TimeSpan(ticks: 0)), TimestampTagStyles.Relative)}\n";
            desc += $"**Level:** {user.Statistics.Level.Current:N0} + {user.Statistics.Level.Progress:N2}%\n";
            desc += $"**PP:** {user.Statistics.Pp:N2} Acc: {user.Statistics.HitAccuracy:N2}%\n";
            desc += $"**Playcount:** {user.Statistics.PlayCount:N0} ({user.Statistics.PlayTime:N0} hrs)\n";
            desc += $"**Grades:** SSH{user.Statistics.GradeCounts.SSH} SS{user.Statistics.GradeCounts.SS} SH{user.Statistics.GradeCounts.SH} S{user.Statistics.GradeCounts.S} A{user.Statistics.GradeCounts.A}\n";
            embed.WithDescription(desc);
            return embed;
        }
        public static EmbedBuilder Embed(this Structures.Beatmap beatmap)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(beatmap.Beatmapset.Artist + " - " + beatmap.Beatmapset.Title);
            embed.WithUrl(beatmap.Url);
            embed.WithThumbnailUrl(beatmap.Beatmapset.Covers.Cover);

            string desc = "";
            desc += $"**Length:** {new TimeOnly(beatmap.TotalLength * 1000).ToString("mm\\:ss")} ({new TimeOnly(beatmap.HitLength * 1000).ToString("mm\\:ss")} drain)";
            desc += $"**BPM:** {beatmap.Bpm:N1}\n";
            desc += $"**Download:** [osu!direct](osu://b/{beatmap.Id}) [web](https://osu.ppy.sh/d/{beatmap.Beatmapset.Id})\n";
            embed.WithDescription(desc);

            string field = "";
            field += $"**Difficulty:** {beatmap.DifficultyRating:N2}★ **Max Combo:** {beatmap.MaxCombo:N0}x\n";
            field += $"**AR:** {beatmap.Ar:N2} **OD:** {-1:N2} **CS:** {beatmap.Cs:N2} **HP:** {beatmap.Drain:N2}\n";
            field += $"**PP:** when im not lazy";
            embed.AddField("Stats", field);

            return embed;
        }
        public static EmbedBuilder Embed(this Structures.Score score, Structures.Beatmap beatmap)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle($"{beatmap.Beatmapset.Title} [{beatmap.DifficultyRating:N2}★]");
            embed.WithUrl(beatmap.Url);
            embed.WithThumbnailUrl(beatmap.Beatmapset.Covers.Cover);

            string desc = "";
            desc += $"**Score:** {score.Score_:N0} ({score.Rank})\n";
            desc += $"**Accuracy:** {score.Accuracy:N2}%\n";
            desc += $"**Combo:** {score.MaxCombo:N0}x/{beatmap.MaxCombo:N0}x\n";
            desc += $"**PP:** {score.Pp:N2}pp\n";
            desc += $"**Mods:** {score.Mods}\n";
            embed.WithDescription(desc);
            return embed;
        }
    }
}