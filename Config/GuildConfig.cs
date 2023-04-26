using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace AstolfoBot.Config
{
    public class GuildConfig
    {
        [JsonConverter(typeof(Converters.DiscordChannelConverter))]
        public ITextChannel? LogChannel { get; set; }

        [JsonConverter(typeof(Converters.DiscordChannelConverter))]
        public ITextChannel? InviteLogChannel { get; set; }

        public AstolfoBot.Modules.Counting.CountingConfig Counting { get; set; } = new();

        public AstolfoBot.Modules.Moderation.WarnData WarnData { get; set; } = new();
    }
}