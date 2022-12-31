using Discord;
using Discord.Interactions;

namespace AstolfoBot.Modules.Other
{
    public class OtherModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("avatar", "Gets the avatar of a user")]
        public async Task AvatarAsync([Summary("user", "The user to get the avatar of")] IUser? user = null)
        {
            user ??= Context.User;
            var embed = new EmbedBuilder()
                .WithAuthor(user)
                .WithImageUrl(user.GetAvatarUrl(ImageFormat.Auto, 2048))
                .WithColor(new Color(0xE26D8F))
                .WithFooter("Avatar");
            await RespondAsync(embed: embed.Build());
        }
    }
}