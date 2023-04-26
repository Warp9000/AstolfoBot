using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace AstolfoBot.Modules.Moderation
{
    public partial class ModerationModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("kick", "Kicks a user")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task
        KickUserAsync(
            [Summary("user", "The user to kick")] SocketGuildUser user,
            [Summary("reason", "The reason for the kick")]
            string reason = "No reason provided"
        )
        {
            if (
                user.Roles.Max(x => x.Position) >=
                Context.Guild.CurrentUser.Roles.Max(x => x.Position)
            )
            {
                var eb =
                    new EmbedBuilder()
                        .WithTitle("I can't kick this user")
                        .WithDescription("The user's highest role is higher than my highest role")
                        .WithColor(Color.Red)
                        .WithCurrentTimestamp()
                        .Build();
                await RespondAsync(embed: eb);
                return;
            }
            reason +=
                $"\n\nModerator: {Context.User.Username}#{Context.User.Discriminator} ({Context.User.Id})";
            await user.KickAsync(reason);
            var embed =
                new EmbedBuilder()
                    .WithTitle("User kicked")
                    .WithDescription($"User: {user.Mention}\nReason: {reason}")
                    .WithColor(Color.Red)
                    .WithCurrentTimestamp()
                    .Build();
            await Logs.LoggerModule.LogToChannel(embed, Context.Guild);
            await RespondAsync(embed: embed);
        }
    }
}
