using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Discord.Rest;

namespace AstolfoBot.Modules.Other
{
    public class OtherModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("avatar", "Gets the avatar of a user")]
        public async Task AvatarAsync([Summary("user", "The user to get the avatar of")] IUser? user = null, [Summary("type", "The type of avatar to get")] AvatarType type = AvatarType.Global)
        {
            user ??= Context.User;
            var embed = new EmbedBuilder()
                .WithAuthor(user)
                .WithImageUrl(type == AvatarType.Global ? user.GetAvatarUrl(size: 4096) : ((SocketGuildUser)user).GetGuildAvatarUrl(size: 4096))
                .WithColor(new Color(0xE26D8F))
                .WithFooter("Avatar");
            await RespondAsync(embed: embed.Build());
        }
        [SlashCommand("banner", "Gets the banner of a user")]
        public async Task BannerAsync([Summary("user", "The user to get the banner of")] IUser? user = null)
        {
            user ??= Context.User;
            RestUser restUser = await Context.Client.Rest.GetUserAsync(user.Id);
            string bannerUrl = restUser.GetBannerUrl(size: 4096);
            if (bannerUrl == null)
            {
                await RespondAsync("This user does not have a banner");
                return;
            }
            var embed = new EmbedBuilder()
                .WithAuthor(user)
                .WithImageUrl(bannerUrl)
                .WithColor(new Color(0xE26D8F))
                .WithFooter("Banner");
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
        [SlashCommand("help", "Shows help for the bot")]

        public async Task HelpAsync([Summary("command", "The command to get help for"), Autocomplete(typeof(AstolfoBot.Completers.CommandAutocompleteHandler))] string? command = null)
        {
            var embed = new EmbedBuilder()
                .WithTitle("Help")
                .WithDescription("Use `/help [command]` to get help for a command")
                .WithColor(new Color(0xE26D8F))
                .WithFooter("Help");
            foreach (var module in Main.CommandHandler.InteractionService.Modules)
            {
                if (module.IsSubModule)
                {
                    continue;
                }
                var group = module.IsSlashGroup ? $"{module.SlashGroupName} " : "";
                var commands = GetCommands(module).Select(x =>
                $"`/{group}{((x.Module.IsSlashGroup && x.Module != module) ? x.Module.SlashGroupName + " " : "")}{x.Name}` - {x.Description}");
                var str = string.Join("\n", commands);
                if (string.IsNullOrEmpty(str))
                    continue;
                var name = module.Name.Replace("Module", "");
                var s = "";
                foreach (var c in name)
                {
                    if (char.IsUpper(c))
                        s += " ";
                    s += c;
                }
                name = s.TrimStart(' ');
                embed.AddField(name, str);
            }
            await RespondAsync(embed: embed.Build());
        }
        private SlashCommandInfo[] GetCommands(ModuleInfo module)
        {
            List<SlashCommandInfo> commands = new();
            foreach (var command in module.SlashCommands)
            {
                commands.Add(command);
            }
            foreach (var subModule in module.SubModules)
            {
                commands.AddRange(GetCommands(subModule));
            }
            return commands.ToArray();
        }
    }
}