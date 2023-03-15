using Discord;
using Newtonsoft.Json;

namespace AstolfoBot.Converters
{
    public class DiscordUserConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IUser);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return (IUser)Main.Client.GetUser(Convert.ToUInt64(reader.Value));
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is IUser user)
            {
                writer.WriteValue(user.Id);
            }
        }
    }
}