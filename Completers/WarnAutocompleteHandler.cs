using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;

namespace AstolfoBot.Completers
{
    public class WarnAutocompleteHandler : AutocompleteHandler
    {
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            var results = new List<AutocompleteResult>();

            var config = context.Guild.GetConfig();
            var warnData = config.WarnData;

            foreach (var warn in warnData.Warns)
            {
                var user = await context.Client.GetUserAsync(warn.UserId);
                if (user == null)
                    continue;
                string str = $"{warn.Id} - {user.Username}#{user.Discriminator} - {warn.Reason}";
                string value = (string)autocompleteInteraction.Data.Current.Value;
                if (value.Length == 0 || str.Contains(value))
                    results.Add(new AutocompleteResult(str, warn.Id.ToString()));
            }
            await Task.Delay(0);
            return AutocompletionResult.FromSuccess(results.Take(25));
        }
    }
}