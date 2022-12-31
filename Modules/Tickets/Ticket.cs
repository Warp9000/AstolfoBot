using Discord;
using Discord.Interactions;

namespace AstolfoBot.Modules.Tickets
{
    public struct Ticket
    {
        public Ticket(ITextChannel channel, IUser user, uint id, bool isOpen = true)
        {
            Channel = channel;
            User = user;
            Id = id;
            IsOpen = isOpen;
        }
        public ITextChannel Channel { get; }
        public IUser User { get; }
        public uint Id { get; }
        public bool IsOpen { get; set; }
    }
}