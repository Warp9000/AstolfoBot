using System.Reflection.Emit;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;
using Newtonsoft.Json;

namespace AstolfoBot.Modules.Emotes
{
    public class EmotesAutocomplete : AutocompleteHandler
    {
        private readonly string[] types;
        public EmotesAutocomplete()
        {
            if (!Directory.Exists("images"))
                Directory.CreateDirectory("images");
            types = Directory.GetDirectories("images").Select(x => Path.GetFileName(x)).ToArray();
        }
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            var suggestions = new List<AutocompleteResult>();
            foreach (var type in types)
            {
                if (type.Contains(autocompleteInteraction.Data.Current.Value.ToString() ?? "", StringComparison.OrdinalIgnoreCase))
                    suggestions.Add(new AutocompleteResult(type, type));
            }
            await Task.Delay(0);
            return AutocompletionResult.FromSuccess(suggestions.Take(25));
        }
    }
}