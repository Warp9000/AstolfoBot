using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Modules.Tests;

namespace AstolfoBot.Modules.Fun
{
    [Group("meter", "Measures how much of something you are")]
    public class MeterModule : InteractionModuleBase<SocketInteractionContext>
    {
        private int Hash(string s)
        {
            var hash = 0;
            foreach (var c in s)
            {
                hash = ((hash << 5) - hash) + c;
                hash |= 0;
            }
            return hash;
        }

        [SlashCommand("gay", "How gay are you?")]
        public async Task Gay(IUser? user = null)
        {
            user ??= Context.User;
            var hash = Math.Abs(Hash(user.Id + "1108"));
            var gayness = (hash % 1001) / 10f;
            var embed = new EmbedBuilder()
                .WithColor(Color.Purple)
                .WithTitle($"{user.Username} is {gayness}% gay")
                .WithDescription("`[" + new string('|', (int)gayness / 2) + new string('.', 50 - (int)gayness / 2) + "]`")
                .WithCurrentTimestamp()
                .WithFooter("These values are not accurate and are just for fun")
                .Build();
            await RespondAsync(embed: embed);
        }

        [SlashCommand("furry", "How much of a furry are you?")]
        public async Task Furry(IUser? user = null)
        {
            user ??= Context.User;
            var hash = Math.Abs(Hash(user.Id + "930"));
            var furryness = (hash % 1001) / 10f;
            var embed = new EmbedBuilder()
                .WithColor(Color.Purple)
                .WithTitle($"{user.Username} is {furryness}% of a furry")
                .WithDescription("`[" + new string('|', (int)furryness / 2) + new string('.', 50 - (int)furryness / 2) + "]`")
                .WithCurrentTimestamp()
                .WithFooter("These values are not accurate and are just for fun")
                .Build();
            await RespondAsync(embed: embed);
        }

        [SlashCommand("horny", "How horny are you?")]
        public async Task Horny(IUser? user = null)
        {
            user ??= Context.User;
            var hash = Math.Abs(Hash(user.Id + "335"));
            var horniness = (hash % 1001) / 10f;
            var embed = new EmbedBuilder()
                .WithColor(Color.Purple)
                .WithTitle($"{user.Username} is {horniness}% horny")
                .WithDescription("`[" + new string('|', (int)horniness / 2) + new string('.', 50 - (int)horniness / 2) + "]`")
                .WithCurrentTimestamp()
                .WithFooter("These values are not accurate and are just for fun")
                .Build();
            await RespondAsync(embed: embed);
        }

        [SlashCommand("ppsize", "Shlong")]
        public async Task PPSize(IUser? user = null)
        {
            user ??= Context.User;
            var hash = Math.Abs(Hash(user.Id + "131"));
            var horniness = (hash % 1001) / 10f;
            var embed = new EmbedBuilder()
                .WithColor(Color.Purple)
                .WithTitle($"{user.Username}'s pp is {horniness}cm long")
                .WithDescription("8" + new string('=', (int)horniness / 2) + "D")
                .WithCurrentTimestamp()
                .WithFooter("These values are not accurate and are just for fun")
                .Build();
            await RespondAsync(embed: embed);
        }

        [SlashCommand("findseed", "Finds the seed that would give a certain percentage")]
        [RequireBotAdmin]
        public async Task FindSeed(float DesiredPercentage, string DesiredSeedStart)
        {
            await RespondAsync("Calculating");
            int i = 0;
            while (true)
            {
                var hash = Math.Abs(Hash(DesiredSeedStart + i.ToString()));
                var percent = (hash % 1001) / 10f;
                if (percent == DesiredPercentage)
                {
                    await ModifyOriginalResponseAsync(x => x.Content = $"Seed: {i}");
                    break;
                }
                i++;
                if (i % 1000 == 0)
                {
                    await ModifyOriginalResponseAsync(x => x.Content = $"Calculating... {i} iterations");
                }
            }
        }
    }
}