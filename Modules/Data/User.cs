using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Discord.Rest;
using AstolfoBot.Completers;
using AstolfoBot.Modules.Tests;
using System.Reflection;

namespace AstolfoBot.Modules.Data
{
    [Group("data", "Data commands")]
    public partial class DataModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("user", "Gets data about a user")]
        [RequireTester]
        public async Task UserAsync(SocketGuildUser? user = null, bool extensive = false)
        {
            user ??= (SocketGuildUser)Context.User;

            var embed = new EmbedBuilder()
                .WithAuthor(user)
                .WithColor(new Color(0xE26D8F))
                .WithFooter("User Data")
                .WithThumbnailUrl(user.GetAvatarUrl(size: 4096));
            embed.AddField("ID", user.Id, true);
            embed.AddField("Dates",
                "Joined: " + user.JoinedAt?.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\n" +
                "Created: " + user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss.fff"), true);
            embed.AddField($"Roles ({user.Roles.Count})", string.Join(", ", user.Roles.Select(x => x.Mention)), true);

            if (extensive)
            {
                embed.AddField("Status", user.Status, true);
                embed.AddField("Activities", string.Join(", ", user.Activities.Select(x => x.Name)), true);
                embed.AddField("Boosting", user.PremiumSince == null ? "Not boosting" : "Boosting since " + user.PremiumSince.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), true);
                embed.AddField("Flags", "Public Flags: " + user.PublicFlags + "\nGuild Flags: " + user.Flags, true);
                embed.AddField("Voice",
                    "Voice Channel: " + user.VoiceChannel?.Name +
                    "\nDeafened: " + user.IsDeafened +
                    "\nMuted: " + user.IsMuted +
                    "\nSelf Deafened: " + user.IsSelfDeafened +
                    "\nSelf Muted: " + user.IsSelfMuted +
                    "\nSuppressed: " + user.IsSuppressed, true);
                embed.AddField("Boolean Properties",
                    "Is Bot: " + user.IsBot +
                    "\nIs Webhook: " + user.IsWebhook +
                    "\nIs Streaming: " + user.IsStreaming +
                    "\nIs Videoing: " + user.IsVideoing +
                    "\nIs Pending: " + user.IsPending, true);
                embed.AddField("Permissions", string.Join(", ", user.GuildPermissions.ToList().Select(x => x.ToString())), false);
            }

            await RespondAsync(embed: embed.Build());
        }
    }
}