using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace AstolfoBot.Modules.Other
{
    public class OtherModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("avatar", "Gets the avatar of a user")]
        public async Task AvatarAsync([Summary("user", "The user to get the avatar of")] IUser? user = null, [Summary("type", "The type of avatar to get")] AvatarType type = AvatarType.Global)
        {
            // https://cdn.discordapp.com/guilds/990296400497631323/users/555218967954980874/avatars/cb4c4d028328368c8dd7dbb54749e650.png?size=4096
            //                                   990296400497631323       555218967954980874
            user ??= Context.User;
            var embed = new EmbedBuilder()
                .WithAuthor(user)
                .WithImageUrl(type == AvatarType.Global ? user.GetAvatarUrl(size: 4096) : ((SocketGuildUser)user).GetGuildAvatarUrl(size: 4096))
                .WithColor(new Color(0xE26D8F))
                .WithFooter("Avatar");
            await RespondAsync(embed: embed.Build());
        }
        public enum AvatarType
        {
            Global,
            Server
        }
        [SlashCommand("membercount", "Gets the user count of the server")]
        public async Task MemberCountAsync()
        {
            var embed = new EmbedBuilder()
                .WithAuthor(Context.Guild.Name)
                .WithColor(new Color(0xE26D8F))
                .WithFooter("Member Count");
            var users = Context.Guild.Users;
            var bots = users.Where(x => x.IsBot).Count();
            var humans = users.Where(x => !x.IsBot).Count();
            embed.WithDescription($"Humans: {humans}\nBots: {bots}\nTotal: {users.Count}");
            await RespondAsync(embed: embed.Build());
        }
    }
}