using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;

namespace AstolfoBot.Modules.Tickets
{
    public class TicketIdAutocomplete : AutocompleteHandler
    {
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            var tickets = (context.Guild as SocketGuild)?.GetConfig().Tickets;
            var ticketIds = tickets?.Select(ticket => ticket.Id.ToString()) ?? Enumerable.Empty<string>();
            List<AutocompleteResult> results = new();
            foreach (var ticketId in ticketIds)
            {
                results.Add(new AutocompleteResult(ticketId, ticketId));
            }
            await Task.Delay(0);
            return AutocompletionResult.FromSuccess(results.Take(25));
        }
    }
}