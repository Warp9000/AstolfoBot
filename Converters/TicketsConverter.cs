using Discord;
using Newtonsoft.Json;

namespace AstolfoBot.Converters
{
    public class TicketsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<Modules.Tickets.Ticket>);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var tickets = new List<Modules.Tickets.Ticket>();
            if (reader.TokenType == JsonToken.StartArray)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.EndArray)
                    {
                        break;
                    }
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        var channel = Main.Client.GetChannel(Convert.ToUInt64(reader.ReadAsString())) as ITextChannel ?? throw new Exception("Channel is not a text channel");
                        reader.Read();
                        var user = Main.Client.GetUser(Convert.ToUInt64(reader.ReadAsString()));
                        reader.Read();
                        var id = Convert.ToUInt32(reader.ReadAsString());
                        reader.Read();
                        var isOpen = Convert.ToBoolean(reader.ReadAsString());
                        reader.Read();
                        tickets.Add(new Modules.Tickets.Ticket(channel, user, id, isOpen));
                    }
                }
            }
            return tickets;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is List<Modules.Tickets.Ticket> tickets)
            {
                writer.WriteStartArray();
                foreach (var ticket in tickets)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("Channel");
                    writer.WriteValue(ticket.Channel.Id);
                    writer.WritePropertyName("User");
                    writer.WriteValue(ticket.User.Id);
                    writer.WritePropertyName("Id");
                    writer.WriteValue(ticket.Id);
                    writer.WritePropertyName("IsOpen");
                    writer.WriteValue(ticket.IsOpen);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
        }
    }
}