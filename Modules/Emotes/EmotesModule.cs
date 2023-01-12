using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace AstolfoBot.Modules.Emotes
{
    // [Group("emotes", "Sends a gif")]
    public class EmotesModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("emote", "Sends a gif")]
        public async Task Emote([Autocomplete(typeof(EmotesAutocomplete))] string type)
        {
            var files = Directory.GetFiles($"images/{type}");
            var file = files[new Random().Next(files.Length)];
            await RespondWithFileAsync(file, file);
        }
    }
}