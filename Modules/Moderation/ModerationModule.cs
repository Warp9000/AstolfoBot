using Newtonsoft.Json;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace AstolfoBot.Modules.Moderation
{
    public class ModerationModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ban", "Bans a user")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(
            [Summary("user", "The user to ban")] SocketGuildUser user,
            [Summary("reason", "The reason for the ban")] string reason = "No reason provided",
            [Summary("deleteDays", "The number of days of messages to delete")] int deleteDays = 0)
        {
            if (user.Roles.Max(x => x.Position) >= Context.Guild.CurrentUser.Roles.Max(x => x.Position))
            {
                var eb = new EmbedBuilder()
                    .WithTitle("I can't ban this user")
                    .WithDescription("The user's highest role is higher than my highest role")
                    .WithColor(Color.Red)
                    .WithCurrentTimestamp()
                    .Build();
                await RespondAsync(embed: eb);
                return;
            }
            reason += $"\n\nModerator: {Context.User.Username}#{Context.User.Discriminator} ({Context.User.Id})";
            await user.BanAsync(deleteDays, reason);
            var embed = new EmbedBuilder()
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
        public async Task UnbanUserAsync(
            [Summary("user", "The user to unban")] SocketUser user,
            [Summary("reason", "The reason for the unban")] string reason = "No reason provided")
        {
            reason += $"\n\nModerator: {Context.User.Username}#{Context.User.Discriminator} ({Context.User.Id})";
            await Context.Guild.RemoveBanAsync(user, new RequestOptions() { AuditLogReason = reason });
            var embed = new EmbedBuilder()
                .WithTitle("User unbanned")
                .WithDescription($"User: {user.Mention}\nReason: {reason}")
                .WithColor(Color.Green)
                .WithCurrentTimestamp()
                .Build();
            await Logs.LoggerModule.LogToChannel(embed, Context.Guild);
            await RespondAsync(embed: embed);
        }

        [SlashCommand("kick", "Kicks a user")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task KickUserAsync(
            [Summary("user", "The user to kick")] SocketGuildUser user,
            [Summary("reason", "The reason for the kick")] string reason = "No reason provided")
        {
            if (user.Roles.Max(x => x.Position) >= Context.Guild.CurrentUser.Roles.Max(x => x.Position))
            {
                var eb = new EmbedBuilder()
                    .WithTitle("I can't kick this user")
                    .WithDescription("The user's highest role is higher than my highest role")
                    .WithColor(Color.Red)
                    .WithCurrentTimestamp()
                    .Build();
                await RespondAsync(embed: eb);
                return;
            }
            reason += $"\n\nModerator: {Context.User.Username}#{Context.User.Discriminator} ({Context.User.Id})";
            await user.KickAsync(reason);
            var embed = new EmbedBuilder()
                .WithTitle("User kicked")
                .WithDescription($"User: {user.Mention}\nReason: {reason}")
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build();
            await Logs.LoggerModule.LogToChannel(embed, Context.Guild);
            await RespondAsync(embed: embed);
        }

        [SlashCommand("mute", "Mutes a user")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task MuteUserAsync(
            [Summary("user", "The user to mute")] SocketGuildUser user,
            [Summary("reason", "The reason for the mute")] string reason = "No reason provided",
            [Summary("duration", "The duration of the mute")] TimeSpan? duration = null)
        {
            if (user.Roles.Max(x => x.Position) >= Context.Guild.CurrentUser.Roles.Max(x => x.Position))
            {
                var eb = new EmbedBuilder()
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
                .WithDescription(
                    $"User: {user.Mention}\n" +
                    $"Duration: {duration.Value.ToString($"dd' days, 'hh' hours, 'mm' minutes, 'ss' seconds'")}\n" +
                    $"Reason: {reason}")
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build();
            await Logs.LoggerModule.LogToChannel(embed, Context.Guild);
            await RespondAsync(embed: embed);
        }

        [SlashCommand("unmute", "Unmutes a user")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task UnmuteUserAsync(
            [Summary("user", "The user to unmute")] SocketGuildUser user,
            [Summary("reason", "The reason for the unmute")] string reason = "No reason provided")
        {
            if (user.Roles.Max(x => x.Position) >= Context.Guild.CurrentUser.Roles.Max(x => x.Position))
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

            await user.RemoveTimeOutAsync(new RequestOptions { AuditLogReason = reason });

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