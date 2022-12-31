using Discord;
using Newtonsoft.Json;

namespace AstolfoBot.Converters
{
    public class DiscordChannelConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ITextChannel);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return Main.Client.GetChannel(Convert.ToUInt64(reader.Value)) as ITextChannel;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is ITextChannel channel)
            {
                writer.WriteValue(channel.Id);
            }
        }
    }
}