using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace AstolfoBot.Modules.Moderation
{
    public partial class ModerationModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("mute", "Mutes a user")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task
        MuteUserAsync(
            [Summary("user", "The user to mute")] SocketGuildUser user,
            [Summary("reason", "The reason for the mute")] string reason = "No reason provided",
            [Summary("duration", "The duration of the mute")] TimeSpan? duration = null
        )
        {
            if (user.Roles.Max(x => x.Position) >= Context.Guild.CurrentUser.Roles.Max(x => x.Position))
            {
                var eb =
                    new EmbedBuilder()
                        .WithTitle("I can't mute this user")
                        .WithDescription("The user's highest role is higher than my highest role")
                        .WithColor(Color.Red)
                        .WithCurrentTimestamp()
                        .Build();
                await RespondAsync(embed: eb);
                return;
            }

            reason += $"\n\nModerator: {Context.User.Username}#{Context.User.Discriminator} ({Context.User.Id})";

            if (!duration.HasValue)
            {
                duration = TimeSpan.FromDays(28);
            }

            await user.SetTimeOutAsync(duration.Value, new RequestOptions { AuditLogReason = reason });

            var embed = new EmbedBuilder()
                .WithTitle("User muted")
                .WithDescription($"User: {user.Mention}\n" +
                $"Duration: {duration
                        .Value:dd' days, 'hh' hours, 'mm' minutes, 'ss' seconds'}\n" +
                $"Reason: {reason}")
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build();
            await Logs.LoggerModule.LogToChannel(embed, Context.Guild);
            await RespondAsync(embed: embed);
        }

        [SlashCommand("unmute", "Unmutes a user")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task
        UnmuteUserAsync(
            [Summary("user", "The user to unmute")] SocketGuildUser user,
            [Summary("reason", "The reason for the unmute")] string reason = "No reason provided"
        )
        {
            if (
                user.Roles.Max(x => x.Position) >=
                Context.Guild.CurrentUser.Roles.Max(x => x.Position)
            )
            {
                var eb = new EmbedBuilder()
                    .WithTitle("I can't unmute this user")
                    .WithDescription("The user's highest role is higher than my highest role")
                    .WithColor(Color.Red)
                    .WithCurrentTimestamp()
                    .Build();
                await RespondAsync(embed: eb);
                return;
            }

            if (user.TimedOutUntil == null)
            {
                var eb = new EmbedBuilder()
                    .WithTitle("User not muted")
                    .WithColor(Color.Red)
                    .WithCurrentTimestamp()
                    .Build();
                await RespondAsync(embed: eb);
                return;
            }

            reason += $"\n\nModerator: {Context.User.Username}#{Context.User.Discriminator} ({Context.User.Id})";

            await user
                .RemoveTimeOutAsync(new RequestOptions
                {
                    AuditLogReason = reason
                });

            var embed = new EmbedBuilder()
                .WithTitle("User unmuted")
                .WithDescription($"User: {user.Mention}\nReason: {reason}")
                .WithColor(Color.Green)
                .WithCurrentTimestamp()
                .Build();
            await Logs.LoggerModule.LogToChannel(embed, Context.Guild);
            await RespondAsync(embed: embed);
        }
    }
}
