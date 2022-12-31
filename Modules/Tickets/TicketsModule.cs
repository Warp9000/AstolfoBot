using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;

namespace AstolfoBot.Modules.Tickets
{
    [Group("tickets", "Commands for managing tickets")]
    public class TicketsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("create", "Creates a new ticket")]
        public async Task CreateTicket([Summary("reason", "The reason for the ticket")] string reason)
        {
            var c = await Tickets.CreateTicket(Context.User, Context.Guild, reason);
            await RespondAsync($"Ticket created: {c.Mention}");
        }

        [SlashCommand("close", "Closes a ticket")]
        public async Task CloseTicket([Autocomplete(typeof(TicketIdAutocomplete))] uint ticketId)
        {
            await Tickets.CloseTicket(Context.Guild, ticketId);
            await RespondAsync($"Ticket {ticketId} closed");
        }

        [Group("admin", "Commands for managing tickets")]
        [DefaultMemberPermissions(GuildPermission.ManageChannels)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public class AdminModule : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("create_embed", "Creates a new ticket embed")]
            public async Task CreateTicketEmbed(ITextChannel channel)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("New Ticket")
                    .WithDescription("Click the button below to create a new ticket")
                    .WithColor(Color.Blue)
                    .Build();
                ButtonBuilder button = new()
                {
                    Label = "Create Ticket",
                    Style = ButtonStyle.Primary,
                    CustomId = "create_ticket",
                    Emote = new Emoji("📩")
                };
                var message = await channel.SendMessageAsync(embed: embed, components: new ComponentBuilder().WithButton(button).Build());
                await RespondAsync("Created ticket embed in " + channel.Name, ephemeral: true);
            }

            [SlashCommand("set_open_category", "Sets the category for open tickets")]
            public async Task SetTicketCategory(SocketCategoryChannel category)
            {
                var cfg = Context.Guild.GetConfig();
                cfg.OpenTicketCategory = category;
                cfg.SaveConfig(Context.Guild);
                await RespondAsync("Set open ticket category to " + category.Name);
            }

            [SlashCommand("set_closed_category", "Sets the category for closed tickets")]
            public async Task SetClosedTicketCategory(SocketCategoryChannel category)
            {
                var cfg = Context.Guild.GetConfig();
                cfg.ClosedTicketCategory = category;
                cfg.SaveConfig(Context.Guild);
                await RespondAsync("Set closed ticket category to " + category.Name);
            }

            [SlashCommand("add_user", "Adds a user to a ticket")]
            public async Task AddUserToTicket(IUser user, [Autocomplete(typeof(TicketIdAutocomplete))] uint ticketId)
            {
                await Tickets.AddUserToTicket(Context.Guild, ticketId, user);
            }
        }
    }
}