using Discord;
using Discord.Interactions;
using Newtonsoft.Json;

namespace AstolfoBot.Modules.Tests
{
    public class TestModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Gets the bot's latency")]
        public async Task TestCommand()
        {
            await RespondAsync($"Pong! {Context.Client.Latency}ms");
        }
        [SlashCommand("typeof", "Gets the type of a variable")]
        [Tests.RequireTester]
        public async Task TypeOfCommand(ITextChannel? channel = null, IUser? user = null, IRole? role = null)
        {
            // if (!IsOwner())
            // {
            //     await RespondAsync("You are not the owner of this bot!");
            //     return;
            // }
            await RespondAsync($"Channel: {channel?.GetType()} User: {user?.GetType()} Role: {role?.GetType()}");
        }
        [SlashCommand("jsonembed", "Sends an embed from a JSON string")]
        [Tests.RequireTester]
        public async Task JsonEmbedCommand(string json = "")
        {
            // if (!IsOwner())
            // {
            //     await RespondAsync("You are not the owner of this bot!");
            //     return;
            // }
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    var eb = new EmbedBuilder()
                        .WithTitle("Title")
                        .WithDescription("Description")
                        .AddField("Field1", "Value1")
                        .AddField("Field2", "Value2")
                        .WithFooter("Footer")
                        .WithColor(Color.Blue)
                        .WithAuthor("Author", "https://cdn.discordapp.com/avatars/1055586888884428941/41b37cd5ff72ab7f40ee99bb9824581c.webp", "https://discord.com")
                        .WithThumbnailUrl("https://cdn.discordapp.com/avatars/1055586888884428941/41b37cd5ff72ab7f40ee99bb9824581c.webp")
                        .WithImageUrl("https://cdn.discordapp.com/avatars/1055586888884428941/41b37cd5ff72ab7f40ee99bb9824581c.webp")
                        .WithCurrentTimestamp()
                        .WithUrl("https://discord.com");
                    await RespondAsync(JsonConvert.SerializeObject("```json\n" + eb + "\n```", Formatting.Indented));
                    return;
                }
                var embed = JsonConvert.DeserializeObject<EmbedBuilder>(json)!;
                await RespondAsync(embed: embed.Build());
            }
            catch (Exception e)
            {
                await RespondAsync(JsonConvert.SerializeObject(e, Formatting.Indented));
            }
        }
        [SlashCommand("exception", "it just throws an exception")]
        public static async Task ExceptionCommand()
        {
            await Task.Delay(0);
            throw new Exception("This is an exception");
        }
    }
}