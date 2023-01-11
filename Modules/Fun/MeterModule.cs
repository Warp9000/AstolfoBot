using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace AstolfoBot.Modules.Fun
{
    [Group("meter", "Measures how much of something you are")]
    public class MeterModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("gay", "how gay are you?")]
        public async Task Gay(IUser? user = null)
        {
            user ??= Context.User;
            var rng = new Random((user.Id + "gayometer").GetHashCode());
            var gayness = rng.Next(0, 101);
            var embed = new EmbedBuilder()
                .WithColor(Color.Purple)
                .WithTitle($"{user.Username} is {gayness}% gay")
                .WithDescription("`[" + new string('|', gayness / 2) + new string('.', 50 - gayness / 2) + "]`")
                .WithCurrentTimestamp()
                .WithFooter("These values are not accurate and are just for fun")
                .Build();
            await RespondAsync(embed: embed);
        }

        [SlashCommand("furry", "how much of a furry are you?")]
        public async Task Furry(IUser? user = null)
        {
            user ??= Context.User;
            var rng = new Random((user.Id + "furryometer").GetHashCode());
            var furryness = rng.Next(0, 101);
            var embed = new EmbedBuilder()
                .WithColor(Color.Purple)
                .WithTitle($"{user.Username} is {furryness}% of a furry")
                .WithDescription("`[" + new string('|', furryness / 2) + new string('.', 50 - furryness / 2) + "]`")
                .WithCurrentTimestamp()
                .WithFooter("These values are not accurate and are just for fun")
                .Build();
            await RespondAsync(embed: embed);
        }

        [SlashCommand("horny", "how horny are you?")]
        public async Task Horny(IUser? user = null)
        {
            user ??= Context.User;
            var rng = new Random((user.Id + "sex :)").GetHashCode());
            var horniness = rng.Next(0, 101);
            var embed = new EmbedBuilder()
                .WithColor(Color.Purple)
                .WithTitle($"{user.Username} is {horniness}% horny")
                .WithDescription("`[" + new string('|', horniness / 2) + new string('.', 50 - horniness / 2) + "]`")
                .WithCurrentTimestamp()
                .WithFooter("These values are not accurate and are just for fun")
                .Build();
            await RespondAsync(embed: embed);
        }
    }
}