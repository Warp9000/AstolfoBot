using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace AstolfoBot.Modules.MathM
{
    [Group("math", "Math commands")]
    public class MathModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("add", "Adds two numbers")]
        public async Task AddCommand([Summary("a", "First number")] string a, [Summary("b", "Second number")] string b)
        {
            bool s1 = double.TryParse(a, out var aNum);
            bool s2 = double.TryParse(b, out var bNum);
            if (!s1 || !s2)
            {
                await RespondAsync("Invalid Input");
                return;
            }
            await RespondAsync($"{a} + {b} = {aNum + bNum}");
        }

        [SlashCommand("parse", "Parses a math expression")]
        public async Task ParseCommand([Summary("expression", "The expression to parse")] string expression)
        {
            var result = MathParser.Parse(expression);
            await RespondAsync(result.ToString());
        }

        [SlashCommand("help", "Shows help for the math module")]
        public async Task HelpCommand()
        {
            var embed = new EmbedBuilder()
                .WithColor(Color.Purple)
                .WithTitle("Math Module Help")
                .WithDescription("This module contains math commands")
                .AddField("Functions", "These are the functions that can be used in the parse command\n" +
                MathParser.Functions.Aggregate("", (s, f) => s + $"`{f.Key}` - {f.Value.Method.DeclaringType}.{f.Value.Method.Name}()\n"))
                .WithCurrentTimestamp()
                .Build();
            await RespondAsync(embed: embed);
        }
    }
}