using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Modules.Tests;

namespace AstolfoBot.Modules.Statistics
{
    [Group("statistics", "Commands for getting statistics")]
    [RequireBotAdmin]
    public class StatisticsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [Group("guild", "Commands for getting guild statistics")]
        public class GuildStatisticsModule : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("count", "Gets the amount of guilds the bot is in")]
            public async Task CountAsync()
            {
                await RespondAsync($"I am in {Main.Client.Guilds.Count} guilds");
            }
        }

        [Group("user", "Commands for getting user statistics")]
        public class UserStatisticsModule : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("count", "Gets the amount of users the bot can see")]
            public async Task CountAsync()
            {
                await RespondAsync($"I can see {Main.Client.Guilds.Sum(x => x.MemberCount)} users");
            }

            [SlashCommand("statuses", "Gets the amount of users with each status")]
            public async Task StatusesAsync([Summary("online-only")] bool onlineOnly = true)
            {
                var statuses = new Dictionary<Discord.UserStatus, int>();
                foreach (var guild in Main.Client.Guilds)
                {
                    foreach (var user in guild.Users)
                    {
                        if (statuses.ContainsKey(user.Status))
                        {
                            statuses[user.Status]++;
                        }
                        else
                        {
                            statuses.Add(user.Status, 1);
                        }
                    }
                }
                if (onlineOnly)
                {
                    statuses.Remove(Discord.UserStatus.Offline);
                    statuses.Remove(Discord.UserStatus.Invisible);
                }
                int total = statuses.Sum(x => x.Value);
                var embed = new EmbedBuilder()
                    .WithTitle("User Statuses")
                    .WithColor(Color.Blue)
                    .WithFooter("User Statuses");
                foreach (var status in statuses)
                {
                    embed.AddField(status.Key.ToString(), status.Value + " (" + Math.Round((double)status.Value / total * 100, 2) + "%)");
                }
                await RespondAsync(embed: embed.Build());
            }
            [SlashCommand("createdat", "Gets the amount of users created at each year")]
            public async Task CreatedAtAsync()
            {
                var created = new Dictionary<int, int>();
                foreach (var guild in Main.Client.Guilds)
                {
                    foreach (var user in guild.Users)
                    {
                        if (created.ContainsKey(user.CreatedAt.Year))
                        {
                            created[user.CreatedAt.Year]++;
                        }
                        else
                        {
                            created.Add(user.CreatedAt.Year, 1);
                        }
                    }
                }
                int total = created.Sum(x => x.Value);
                created = created.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                var embed = new EmbedBuilder()
                    .WithTitle("User Created At")
                    .WithColor(Color.Blue)
                    .WithFooter("User Created At");
                foreach (var status in created)
                {
                    embed.AddField(
                        status.Key.ToString(),
                        status.Value + " (" + Math.Round((double)status.Value / total * 100, 2) + "%)",
                        true);
                }
                await RespondAsync(embed: embed.Build());
            }
        }
    }
}