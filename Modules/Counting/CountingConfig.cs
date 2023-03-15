using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Converters;
using Newtonsoft.Json;

namespace AstolfoBot.Modules.Counting
{
    public class CountingConfig
    {
        [JsonConverter(typeof(Converters.DiscordChannelConverter))]
        public ITextChannel? CountingChannel { get; set; }

        public int CurrentNumber { get; set; }

        [JsonConverter(typeof(Converters.DiscordUserConverter))]
        public IUser? LastUser { get; set; }
    }
}