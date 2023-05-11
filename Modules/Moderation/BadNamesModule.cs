using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace AstolfoBot.Modules.Moderation
{
    [Group("badnames", "Commands for managing bad names")]
    public class BadNamesModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("list", "List all users with bad names")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task listAsync()
        {
            var guild = Context.Guild;
            var users = guild.Users;
            var badNames = new List<SocketGuildUser>();
            foreach (var user in users)
            {
                // atleast 50% of the characters must be ascii
                var asciiCount = (user.Nickname ?? user.Username).Count(c => c < 128);
                if (asciiCount < (user.Nickname ?? user.Username).Length / 2)
                {
                    badNames.Add(user);
                }
            }
            // send file
            // var file = new MemoryStream();
            // using (var writer = new StreamWriter(file, leaveOpen: true))
            // {
            //     foreach (var badName in badNames)
            //     {
            //         await writer.WriteLineAsync($"{badName.Key} {badName.Value}");
            //     }
            // }
            // file.Position = 0;
            // await RespondWithFileAsync(file, "badnames.txt");

            // send embed
            var embed = new EmbedBuilder()
                .WithTitle("Bad names")
                .WithDescription("Users with bad names\n\n")
                .WithColor(Color.Red)
                .WithFooter("Copy the list and send it yourself for names to resolve");
            foreach (var badName in badNames)
            {
                embed.Description += badName.Mention + " " + (badName.Nickname ?? badName.Username) + "\n";
                if (embed.Description.Length > 2000)
                {
                    embed.Description = embed.Description.Substring(0, 2000);
                    break;
                }
            }
            await RespondAsync(embed: embed.Build());
        }
    }
}