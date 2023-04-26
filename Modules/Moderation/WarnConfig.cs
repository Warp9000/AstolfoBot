using Newtonsoft.Json;

namespace AstolfoBot.Modules.Moderation
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WarnData
    {
        [JsonProperty("warns")]
        public List<Warn> Warns { get; set; } = new();

        public class Warn
        {
            [JsonProperty("id")]
            public Guid Id { get; set; }

            [JsonProperty("userid")]
            public ulong UserId { get; set; }

            [JsonProperty("reason")]
            public string? Reason { get; set; }

            [JsonProperty("moderator")]
            public ulong Moderator { get; set; }

            [JsonProperty("timestamp")]
            public DateTime Timestamp { get; set; }
        }
    }
}