using System.Diagnostics;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Discord.Rest;

namespace AstolfoBot.Modules.Other
{
    public class OtherModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("avatar", "Gets the avatar of a user")]
        public async Task AvatarAsync([Summary("user", "The user to get the avatar of")] IUser? user = null, [Summary("type", "The type of avatar to get")] AvatarType? type = null)
        {
            await DeferAsync();
            user ??= Context.User;
            var guildUser = (SocketGuildUser)user;
            var url = "";
            if (type == null)
            {
                url = guildUser.GetGuildAvatarUrl(size: 4096) ?? guildUser.GetAvatarUrl(size: 4096) ?? guildUser.GetDefaultAvatarUrl();
            }
            else
            {
                switch (type)
                {
                    case AvatarType.Global:
                        url = guildUser.GetAvatarUrl(size: 4096) ?? guildUser.GetDefaultAvatarUrl();
                        break;
                    case AvatarType.Server:
                        url = guildUser.GetGuildAvatarUrl(size: 4096) ?? guildUser.GetAvatarUrl(size: 4096) ?? guildUser.GetDefaultAvatarUrl();
                        break;
                }
            }
            var embed = new EmbedBuilder()
            .WithTitle($"{user.Username}'s Avatar")
                .WithImageUrl(url)
                .WithColor(new Color(0xE26D8F));
            await FollowupAsync(embed: embed.Build());
        }

        [SlashCommand("banner", "Gets the banner of a user")]
        public async Task BannerAsync([Summary("user", "The user to get the banner of")] IUser? user = null)
        {
            await DeferAsync();
            user ??= Context.User;
            RestGuildUser restGuildUser = await Context.Client.Rest.GetGuildUserAsync(Context.Guild.Id, user.Id);
            string bannerUrl = restGuildUser.GetBannerUrl(size: 4096);
            if (bannerUrl == null)
            {
                await FollowupAsync("This user does not have a banner");
                return;
            }
            var embed = new EmbedBuilder()
            .WithTitle($"{user.Username}'s Banner")
                .WithImageUrl(bannerUrl)
                .WithColor(new Color(0xE26D8F));
            await FollowupAsync(embed: embed.Build());
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

        [SlashCommand("info", "Gets info about the bot")]
        public async Task InfoAsync()
        {
            var uptime = DateTime.Now - Process.GetCurrentProcess().StartTime;
            var embed = new EmbedBuilder()
                .WithTitle("Info")
                .WithColor(new Color(0xE26D8F))
                .WithDescription(
                    "[AstolfoBot](https://github.com/WarpABoi/AstolfoBot) is a general purpose bot made by [Warp](https://github.com/WarpABoi) as a hobby project. " +
                    "It is written in C# using [Discord.Net](https://discordnet.dev/) " +
                    "and is  being actively developed.\n" +
                    "If you have any suggestions or issues, feel free to tell me in the [support server](https://discord.gg/sh6Zvrq4ch) or open an issue on github.")
                .AddField("Version", typeof(Main).Assembly.GetName().Version?.ToString() ?? "Unknown", true)
                .AddField("Uptime",
                    $"{(uptime.Days > 0 ? $"{uptime.Days}d " : "")}" +
                    $"{(uptime.Hours > 0 ? $"{uptime.Hours}h " : "")}" +
                    $"{(uptime.Minutes > 0 ? $"{uptime.Minutes}m " : "")}" +
                    $"{uptime.Seconds}s",
                true)
                .AddField("Guilds", Context.Client.Guilds.Count, true)
                .AddField("Users", Context.Client.Guilds.Sum(x => x.MemberCount), true)
                .AddField("Ping", $"{Context.Client.Latency}ms", true)
                .AddField("Memory", $"{Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024}MB", true);

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