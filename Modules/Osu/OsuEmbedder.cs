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
            embed.WithTitle(beatmap.beatmapset.artist + " - " + beatmap.beatmapset.title);
            embed.WithUrl(beatmap.url);
            embed.WithThumbnailUrl(beatmap.beatmapset.covers.cover);

            string desc = "";
            desc += $"**Length:** {new TimeOnly(beatmap.total_length * 1000).ToString("mm\\:ss")} ({new TimeOnly(beatmap.hit_length * 1000).ToString("mm\\:ss")} drain)";
            desc += $"**BPM:** {beatmap.bpm:N1}\n";
            desc += $"**Download:** [osu!direct](osu://b/{beatmap.id}) [web](https://osu.ppy.sh/d/{beatmap.beatmapset.id})\n";
            embed.WithDescription(desc);

            string field = "";
            field += $"**Difficulty:** {beatmap.difficulty_rating:N2}★ **Max Combo:** {beatmap.max_combo:N0}x\n";
            field += $"**AR:** {beatmap.ar:N2} **OD:** {-1:N2} **CS:** {beatmap.cs:N2} **HP:** {beatmap.drain:N2}\n";
            field += $"**PP:** when im not lazy";
            embed.AddField("Stats", field);

            return embed;
        }
    }
}