using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;

namespace AstolfoBot.Modules.Tickets
{
    public class Tickets
    {
        public static async Task<ITextChannel> CreateTicket(IUser user, SocketGuild guild, string reason)
        {
            var cfg = guild.GetConfig();
            var ticketCategory = cfg.OpenTicketCategory;
            if (ticketCategory is null)
            {
                throw new Exception("Ticket category is not set");
            }
            var ticketChannel = await guild.CreateTextChannelAsync($"ticket-{((cfg.Tickets?.Count ?? 0) + 1).ToString("D3")}", x =>
            {
                x.CategoryId = ticketCategory.Id;
                x.Topic =
                $"Ticket for {user.Mention}\n" +
                $"Reason: {reason}\n" +
                $"{TimestampTag.FromDateTimeOffset(DateTimeOffset.Now, TimestampTagStyles.LongDateTime)}";
            });
            var ticket = new Ticket(ticketChannel, user, (uint)(cfg.Tickets?.Count ?? 0) + 1);
            if (cfg.Tickets is null)
            {
                cfg.Tickets = new List<Ticket>();
            }
            cfg.Tickets.Add(ticket);
            await ticketChannel.SyncPermissionsAsync();
            await ticketChannel.AddPermissionOverwriteAsync(guild.EveryoneRole, new OverwritePermissions(viewChannel: PermValue.Deny));
            await ticketChannel.AddPermissionOverwriteAsync(user, new OverwritePermissions(viewChannel: PermValue.Allow));
            await ticketChannel.SendMessageAsync($"Ticket for {user.Mention} created");
            cfg.SaveConfig(guild);
            return ticketChannel;
        }
        public static async Task CloseTicket(SocketGuild guild, uint ticketId)
        {
            var cfg = guild.GetConfig();
            if (!cfg.Tickets.Exists(x => x.Id == ticketId))
            {
                throw new Exception("Ticket does not exist");
            }
            var ticket = cfg.Tickets.FirstOrDefault(x => x.Id == ticketId);
            cfg.Tickets.RemoveAll(x => x.Id == ticketId);
            ticket.IsOpen = false;
            cfg.Tickets.Add(ticket);
            var ticketChannel = ticket.Channel;
            await ticketChannel.SyncPermissionsAsync();
            await ticketChannel.AddPermissionOverwriteAsync(guild.EveryoneRole, new OverwritePermissions(viewChannel: PermValue.Deny));
            var closedTicketCategory = cfg.ClosedTicketCategory;
            if (closedTicketCategory is null)
            {
                throw new Exception("Closed ticket category is not set");
            }
            await ticketChannel.ModifyAsync(x => x.CategoryId = closedTicketCategory.Id);
            cfg.SaveConfig(guild);
        }
        public static async Task AddUserToTicket(SocketGuild guild, uint ticketId, IUser user)
        {
            var cfg = guild.GetConfig();
            if (!cfg.Tickets.Exists(x => x.Id == ticketId))
            {
                throw new Exception("Ticket does not exist");
            }
            var ticket = cfg.Tickets.FirstOrDefault(x => x.Id == ticketId);
            var ticketChannel = ticket.Channel;
            await ticketChannel.AddPermissionOverwriteAsync(user, new OverwritePermissions(viewChannel: PermValue.Allow));
            cfg.SaveConfig(guild);
        }
    }
}