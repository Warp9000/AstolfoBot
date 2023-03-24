using Discord;
using Discord.Interactions;

namespace AstolfoBot.Completers
{
    public class CommandAutocompleteHandler : AutocompleteHandler
    {
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            var results = new List<AutocompleteResult>();

            foreach (var command in InteractionService.SlashCommands)
            {
                var module = command.Module;
                ModuleInfo? parent = module.IsSubModule ? module.Parent : null;
                string name = $"{(parent != null ? parent.SlashGroupName + " " : "")}{module.SlashGroupName} {command.Name}";
                if (name.Contains(autocompleteInteraction.Data.Current.Name))
                    results.Add(new AutocompleteResult(name, command));
            }
            await Task.Delay(0);
            return AutocompletionResult.FromSuccess(results.Take(25));
        }
    }
}