using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace AstolfoBot.Config
{
    public struct GuildConfig
    {
        [JsonConverter(typeof(Converters.DiscordChannelConverter))]
        public ITextChannel? LogChannel { get; set; }

        [JsonConverter(typeof(Converters.DiscordChannelConverter))]
        public ITextChannel? InviteLogChannel { get; set; }

        [JsonConverter(typeof(Converters.DiscordCategoryConverter))]
        public ICategoryChannel? OpenTicketCategory { get; set; }

        [JsonConverter(typeof(Converters.DiscordCategoryConverter))]
        public ICategoryChannel? ClosedTicketCategory { get; set; }
    }
}