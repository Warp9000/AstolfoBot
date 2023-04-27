using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;
using AstolfoBot.Config;
using AstolfoBot.Completers;

namespace AstolfoBot.Modules.Moderation
{
    public partial class ModerationModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("warn", "Warns a user")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task WarnUserAsync([Summary("user", "The user to warn")] SocketUser user, [Summary("reason", "The reason for the warn")] string? reason = null)
        {
            var config = Context.Guild.GetConfig();
            var warnData = config.WarnData;

            var warn = new WarnData.Warn
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Reason = reason,
                Moderator = Context.User.Id,
                Timestamp = DateTime.UtcNow
            };

            warnData.Warns.Add(warn);

            Context.Guild.SaveConfig(config);

            var embed = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithAuthor(Context.User)
                .WithDescription($"Warned {user.Mention}\nReason: {reason ?? "No reason provided"}")
                .WithFooter($"Warn ID: {warn.Id}")
                .WithTimestamp(warn.Timestamp)
                .Build();

            await RespondAsync(embed: embed);
        }

        [SlashCommand("listwarns", "Shows a user's warns")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task WarnsAsync([Summary("user", "The user to show warns for")] SocketUser user, [Summary("page", "The page of warns to show")] int page = 1)
        {
            var config = Context.Guild.GetConfig();
            var warnData = config.WarnData;

            var warns = warnData.Warns.Where(w => w.UserId == user.Id).ToList();

            if (warns.Count == 0)
            {
                var eb = new EmbedBuilder()
                    .WithColor(Color.Red)
                    .WithDescription($"No warns for {user.Mention}")
                    .WithTimestamp(DateTime.UtcNow)
                    .Build();
                await RespondAsync(embed: eb);
                return;
            }

            var embed = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithDescription($"Warns for {user.Mention}")
                .WithFooter($"Warns: {warns.Count}")
                .WithTimestamp(DateTime.UtcNow);

            if (warns.Count > 25)
            {
                embed.WithDescription($"Warns for {user.Mention} (showing 25 of {warns.Count})");
                warns = warns.Skip((page - 1) * 25).Take(25).ToList();
            }

            foreach (var warn in warns)
            {
                embed.AddField($"Warn ID: {warn.Id}", $"Reason: {warn.Reason ?? "no reason"}\nModerator: <@{warn.Moderator}>\nTimestamp: {warn.Timestamp}");
            }

            await RespondAsync(embed: embed.Build());
        }

        [SlashCommand("getwarn", "Gets a warn")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task GetWarnAsync([Autocomplete(typeof(WarnAutocompleteHandler))][Summary("warnid", "The ID of the warn")] string warnId)
        {
            var config = Context.Guild.GetConfig();
            var warnData = config.WarnData;

            var guid = Guid.Parse(warnId);

            var warn = warnData.Warns.FirstOrDefault(w => w.Id == guid);

            if (warn == null)
            {
                var eb = new EmbedBuilder()
                    .WithColor(Color.Red)
                    .WithDescription($"No warn with ID {warnId}")
                    .WithTimestamp(DateTime.UtcNow)
                    .Build();
                await RespondAsync(embed: eb);
                return;
            }

            var embed = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithDescription($"User: <@{warn.UserId}>\nReason: {warn.Reason ?? "no reason"}\nModerator: <@{warn.Moderator}>\nTimestamp: {warn.Timestamp}")
                .WithFooter($"Warn ID: {warn.Id}")
                .WithTimestamp(DateTime.UtcNow)
                .Build();

            await RespondAsync(embed: embed);
        }
        [SlashCommand("deletewarn", "Deletes a warn")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task DeleteWarnAsync([Autocomplete(typeof(WarnAutocompleteHandler))][Summary("warnid", "The ID of the warn to delete")] string warnId)
        {
            var config = Context.Guild.GetConfig();
            var warnData = config.WarnData;

            var guid = Guid.Parse(warnId);

            var warn = warnData.Warns.FirstOrDefault(w => w.Id == guid);

            if (warn == null)
            {
                var eb = new EmbedBuilder()
                    .WithColor(Color.Red)
                    .WithDescription($"No warn with ID {warnId}")
                    .WithTimestamp(DateTime.UtcNow)
                    .Build();
                await RespondAsync(embed: eb);
                return;
            }

            warnData.Warns.Remove(warn);

            Context.Guild.SaveConfig(config);

            var embed = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithDescription($"Deleted warn with ID {warnId}")
                .WithTimestamp(DateTime.UtcNow)
                .Build();

            await RespondAsync(embed: embed);

        }
    }
}