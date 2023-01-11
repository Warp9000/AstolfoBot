using Discord;

namespace AstolfoBot.Modules.Logs
{
    public struct JsonMessage
    {
        public string Content { get; set; }

        public ulong Id { get; set; }

        public ulong ChannelId { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorDiscriminator { get; set; }

        public ulong AuthorId { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public DateTimeOffset? EditedTimestamp { get; set; }

        public static JsonMessage FromIMessage(IMessage message)
        {
            return new JsonMessage {
                Content = message.Content,
                Id = message.Id,
                ChannelId = message.Channel.Id,
                AuthorUsername = message.Author.Username,
                AuthorDiscriminator = message.Author.Discriminator,
                AuthorId = message.Author.Id,
                Timestamp = message.Timestamp,
                EditedTimestamp = message.EditedTimestamp
            };
        }
    }
}
