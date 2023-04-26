using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;

namespace AstolfoBot.Completers
{
    public class BannedUserAutocomplete : AutocompleteHandler
    {
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            var bannedUsers = await context.Guild.GetBansAsync().FlattenAsync();
            var suggestions = new List<AutocompleteResult>();
            foreach (var bannedUser in bannedUsers)
            {
                suggestions.Add(new AutocompleteResult(bannedUser.User.Username, bannedUser.User.Id));
            }
            return AutocompletionResult.FromSuccess(suggestions.Take(25));
        }
    }
}