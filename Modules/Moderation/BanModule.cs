using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace AstolfoBot.Modules.Moderation
{
    public partial class ModerationModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ban", "Bans a user")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task
        BanUserAsync(
            [Summary("user", "The user to ban")] SocketGuildUser user,
            [Summary("reason", "The reason for the ban")]
            string reason = "No reason provided",
            [Summary("deleteDays", "The number of days of messages to delete")]
            int deleteDays = 0
        )
        {
            if (
                user.Roles.Max(x => x.Position) >=
                Context.Guild.CurrentUser.Roles.Max(x => x.Position)
            )
            {
                var eb =
                    new EmbedBuilder()
                        .WithTitle("I can't ban this user")
                        .WithDescription("The user's highest role is higher than my highest role")
                        .WithColor(Color.Red)
                        .WithCurrentTimestamp()
                        .Build();
                await RespondAsync(embed: eb);
                return;
            }
            reason +=
                $"\n\nModerator: {Context.User.Username}#{Context.User.Discriminator} ({Context.User.Id})";
            await user.BanAsync(deleteDays, reason);
            var embed =
                new EmbedBuilder()
                    .WithTitle("User banned")
                    .WithDescription($"User: {user.Mention}\nReason: {reason}")
                    .WithColor(Color.Red)
                    .WithCurrentTimestamp()
                    .Build();
            await Logs.LoggerModule.LogToChannel(embed, Context.Guild);
            await RespondAsync(embed: embed);
        }

        [SlashCommand("unban", "Unbans a user")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task
        UnbanUserAsync(
            [Autocomplete(typeof(Completers.BannedUserAutocomplete))][Summary("user", "The user to unban")] ulong user,
            [Summary("reason", "The reason for the unban")]
            string reason = "No reason provided"
        )
        {
            reason +=
                $"\n\nModerator: {Context.User.Username}#{Context.User.Discriminator} ({Context.User.Id})";
            await Context
                .Guild
                .RemoveBanAsync(user,
                new RequestOptions() { AuditLogReason = reason });
            var embed =
                new EmbedBuilder()
                    .WithTitle("User unbanned")
                    .WithDescription($"User: {user}\nReason: {reason}")
                    .WithColor(Color.Green)
                    .WithCurrentTimestamp()
                    .Build();
            await Logs.LoggerModule.LogToChannel(embed, Context.Guild);
            await RespondAsync(embed: embed);
        }
    }
}
