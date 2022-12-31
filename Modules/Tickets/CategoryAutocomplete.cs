using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;

namespace AstolfoBot.Modules.Tickets
{
    public class CategoryAutocomplete : AutocompleteHandler
    {
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            var guild = context.Guild as SocketGuild;
            var categories = guild?.CategoryChannels;
            var categoryNames = categories?.Select(category => category.Name) ?? Enumerable.Empty<string>();
            List<AutocompleteResult> results = new();
            foreach (var categoryName in categoryNames)
            {
                results.Add(new AutocompleteResult(categoryName, categoryName));
            }
            await Task.Delay(0);
            return AutocompletionResult.FromSuccess(results.Take(25));
        }
    }
}