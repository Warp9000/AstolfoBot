using AstolfoBot.Config;
using Discord;
using Newtonsoft.Json;

namespace AstolfoBot.Modules.Logs
{
    public static class FileLogManager
    {
        #region Read
        public static List<JsonMessage> GetDeletedMessages(
            ulong guildId,
            ulong channelId,
            (DateTime, DateTime)? timeRange = null
        )
        {
            // var guild = Main.Client.GetGuild(guildId);
            // var channel = guild.GetTextChannel(channelId);
            var path = $"Data/Guilds/{guildId}/{channelId}/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var files = Directory.GetFiles(path, "d????-??-??.json");
            var messages = new List<JsonMessage>();
            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var msgs = JsonConvert.DeserializeObject<List<JsonMessage>>(json) ?? new List<JsonMessage>();
                timeRange ??= (DateTime.Now.AddDays(-1), DateTime.Now);
                var (start, end) = timeRange.Value;
                msgs = msgs.Where(m => m.Timestamp >= start && m.Timestamp <= end).ToList();
                messages.AddRange (msgs);
            }
            return messages;
        }
        public static List<(JsonMessage,JsonMessage)> GetEditedMessages(
            ulong guildId,
            ulong channelId,
            (DateTime, DateTime)? timeRange = null
        )
        {
            // var guild = Main.Client.GetGuild(guildId);
            // var channel = guild.GetTextChannel(channelId);
            var path = $"Data/Guilds/{guildId}/{channelId}/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var files = Directory.GetFiles(path, "e????-??-??.json");
            var messages = new List<(JsonMessage,JsonMessage)>();
            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var msgs = JsonConvert.DeserializeObject<List<(JsonMessage,JsonMessage)>>(json) ?? new List<(JsonMessage,JsonMessage)>();
                timeRange ??= (DateTime.Now.AddDays(-1), DateTime.Now);
                var (start, end) = timeRange.Value;
                msgs = msgs.Where(m => m.Item1.EditedTimestamp >= start && m.Item1.EditedTimestamp <= end).ToList();
                messages.AddRange (msgs);
            }
            return messages;
        }
        #endregion
        #region Write
        public static void WriteDeletedMessages(
            ulong guildId,
            ulong channelId,
            List<JsonMessage> messages
        )
        {
            var path = $"Data/Guilds/{guildId}/{channelId}/";
            foreach (var message in messages)
            {
                var file = $"{path}d{message.Timestamp:yyyy-MM-dd}.json";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!File.Exists(file))
                {
                    File.Create(file).Close();
                }
                List<JsonMessage> msgs = JsonConvert.DeserializeObject<List<JsonMessage>>(File.ReadAllText(file)) ?? new List<JsonMessage>();
                msgs.Add(message);
                var json = JsonConvert.SerializeObject(msgs);
                File.WriteAllText(file, json);
            }
        }
        public static void WriteEditedMessages(
            ulong guildId,
            ulong channelId,
            List<(JsonMessage,JsonMessage)> messages
        )
        {
            var path = $"Data/Guilds/{guildId}/{channelId}/";
            foreach (var message in messages)
            {
                var file = $"{path}e{message.Item1.Timestamp:yyyy-MM-dd}.json";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!File.Exists(file))
                {
                    File.Create(file).Close();
                }
                List<(JsonMessage,JsonMessage)> msgs = JsonConvert.DeserializeObject<List<(JsonMessage,JsonMessage)>>(File.ReadAllText(file)) ?? new List<(JsonMessage,JsonMessage)>();
                msgs.Add(message);
                var json = JsonConvert.SerializeObject(msgs);
                File.WriteAllText(file, json);
            }
        }
        #endregion
    }
}
