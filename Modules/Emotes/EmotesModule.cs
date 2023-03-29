using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace AstolfoBot.Modules.Emotes
{
    public class EmotesModule : InteractionModuleBase<SocketInteractionContext>
    {
        public EmotesModule()
        {

        }
        [SlashCommand("emote", "Sends a gif")]
        public async Task Emote(
            [Summary("type", "The type of emote"), Autocomplete(typeof(EmotesAutocomplete))] string type,
            [Summary("name", "The name of the specific emote you want")] string? name = null
            )
        {
            if (Directory.Exists($"images/{type}") == false)
            {
                await RespondAsync("This emote type does not exist");
                return;
            }
            var files = Directory.GetFiles($"images/{type}");
            if (files.Length == 0)
            {
                await RespondAsync("This emote type does not exist");
                return;
            }
            var file = files[new Random().Next(files.Length)];
            if (name is not null)
            {
                if (File.Exists($"images/{type}/{name}"))
                    file = $"images/{type}/{name}";
                else
                    await RespondAsync("This emote does not exist");
            }
            await RespondWithFileAsync(file, file, Path.GetFileName(file));
        }
    }
}