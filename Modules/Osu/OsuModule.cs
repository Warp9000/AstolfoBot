using System.Text;
using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Modules.Osu.Api;
using AstolfoBot.Config;
using AstolfoBot.Modules.Tests;
using Newtonsoft.Json;

namespace AstolfoBot.Modules.Osu
{
    [Group("osu", "osu! related commands")]
    public class OsuModule : InteractionModuleBase<SocketInteractionContext>
    {
        public static OsuApiWrapper Api { get; set; } = new OsuApiWrapper();
        public OsuModule()
        {
            Api = new OsuApiWrapper();
        }
        [SlashCommand("sendraw", "Sends a raw request to the osu! api")]
        [RequireTester]
        public async Task SendRawAsync(
            [Summary("endpoint", "The endpoint to send the request to")] string endpoint,
            [Summary("lazer", "Whether or not to use the lazer api")] bool lazer = false)
        {
            var response = await Api.GetAsync(endpoint, lazer);
            response = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(response), Formatting.Indented);
            Stream stream = new MemoryStream();
            stream.Write(Encoding.UTF8.GetBytes(response));
            await RespondWithFileAsync(stream, "response.json");
        }
        [Group("user", "osu! user related commands")]
        public class OsuUserGroup : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("info", "Gets information about an osu! user")]
            public async Task InfoAsync(
                [Summary("user", "The user to get information about")] string? user = null,
                [Summary("mode", "The osuStructures.GameMode to get information about")] Structures.GameMode? mode = null)
            {
                user ??= GetUser(Context.User.Id).Id ?? Context.User.Username;
                mode ??= GetUser(Context.User.Id).Mode ?? null;
                var OsuUser = await Api.GetUserAsync(user, mode);
                var embed = OsuUser.Embed();
                await RespondAsync(embed: embed.Build());
            }

            [SlashCommand("recent", "Gets the most recent play of an osu! user")]
            public async Task RecentAsync(
                [Summary("user", "The user to get information about")] string? user = null,
                [Summary("mode", "The osuStructures.GameMode to get information about")] Structures.GameMode? mode = null,
                [Summary("amount", "The amount of plays to get")] int? amount = null,
                [Summary("page", "The page to get")] int? page = null)
            {
                user ??= GetUser(Context.User.Id).Id ?? Context.User.Username;
                mode ??= GetUser(Context.User.Id).Mode ?? null;
                var scores = await Api.GetUserScoresRecentAsync(user, mode, amount, page);
            }

            [SlashCommand("top", "Gets the top plays of an osu! user")]
            public async Task TopAsync(
                [Summary("user", "The user to get information about")] string? user = null,
                [Summary("mode", "The osuStructures.GameMode to get information about")] Structures.GameMode? mode = null,
                [Summary("amount", "The amount of plays to get")] int? amount = null,
                [Summary("page", "The page to get")] int? page = null)
            {
                user ??= GetUser(Context.User.Id).Id ?? Context.User.Username;
                mode ??= GetUser(Context.User.Id).Mode ?? null;
                var scores = await Api.GetUserScoresBestAsync(user, mode, amount, page);
            }

            [SlashCommand("firsts", "Gets the first place plays of an osu! user")]
            public async Task FirstsAsync(
                [Summary("user", "The user to get information about")] string? user = null,
                [Summary("mode", "The osuStructures.GameMode to get information about")] Structures.GameMode? mode = null,
                [Summary("amount", "The amount of plays to get")] int? amount = null,
                [Summary("page", "The page to get")] int? page = null)
            {
                user ??= GetUser(Context.User.Id).Id ?? Context.User.Username;
                mode ??= GetUser(Context.User.Id).Mode ?? null;
                var scores = await Api.GetUserScoresFirstsAsync(user, mode, amount, page);
            }

            [SlashCommand("set", "Sets your default osu! user")]
            public async Task SetAsync(
                [Summary("user", "The user to set")] string? user = null,
                [Summary("mode", "The osuStructures.GameMode to set")] Structures.GameMode? mode = null)
            {
                var config = ConfigManager.GetUserConfig(Context.User.Id);
                config.OsuUser = new OsuFileUser
                {
                    Id = user,
                    Mode = mode
                };
                ConfigManager.SaveUserConfig(Context.User.Id, config);
                await RespondAsync("Your osu! user has been set!");
            }
        }

        [SlashCommand("beatmap", "Gets information about an osu! beatmap")]
        public async Task BeatmapAsync(
            [Summary("beatmap", "The beatmap to get information about")] string? beatmap = null)
        {
            try
            {
                if (ulong.TryParse(beatmap, out var id))
                {
                    var map = await Api.GetBeatmapAsync(id);
                    var embed = /*map.Embed();*/ new EmbedBuilder();
                    await RespondAsync(embed: embed.Build());
                }
                else
                {
                    await RespondAsync("Argument must be a beatmap id!");
                }
            }
            catch (Exception e)
            {
                await RespondAsync(e.Message);
            }
        }

        [SlashCommand("compare", "Get a users scores on the last map in chat")]
        public async Task CompareAsync(
            [Summary("user", "The user to get information about")] string? user = null)
        {
            await RespondAsync("Uh oh, this command isn't implemented yet!");
        }

        public static OsuFileUser GetUser(ulong id)
        {
            var config = ConfigManager.GetUserConfig(id);
            var user = config.OsuUser;
            return user;
        }
        public struct OsuFileUser
        {
            public string? Id { get; set; }
            public Structures.GameMode? Mode { get; set; }
        }
    }
}