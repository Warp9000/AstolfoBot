using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;

namespace AstolfoBot.Modules.Emotes
{
    public class EmotesAutocomplete : AutocompleteHandler
    {
        private readonly string[] types;
        public EmotesAutocomplete()
        {
            types = Directory.GetDirectories("images").Select(x => Path.GetFileName(x)).ToArray();
        }
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            var suggestions = new List<AutocompleteResult>();
            foreach (var type in types)
            {
                suggestions.Add(new AutocompleteResult(type, type));
            }
            await Task.Delay(0);
            return AutocompletionResult.FromSuccess(suggestions.Take(25));
        }
    }
}