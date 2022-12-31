using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace AstolfoBot.Converters
{
    public class SocketInteractionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SocketInteraction);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is SocketInteraction interaction)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("ApplicationId");
                serializer.Serialize(writer, interaction.ApplicationId);
                writer.WritePropertyName("Type");
                serializer.Serialize(writer, interaction.Type);
                writer.WritePropertyName("UserId");
                serializer.Serialize(writer, interaction.User.Id);
                writer.WritePropertyName("ChannelId");
                serializer.Serialize(writer, interaction.ChannelId);
                writer.WritePropertyName("GuildId");
                serializer.Serialize(writer, interaction.GuildId);
                writer.WritePropertyName("Token");
                serializer.Serialize(writer, interaction.Token);
                writer.WriteEndObject();
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return null;
        }
    }
}