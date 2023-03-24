using Newtonsoft.Json;

namespace AstolfoBot.Config
{
    public static class ConfigManager
    {
        public static Dictionary<ulong, GuildConfig> GuildConfig { get; set; } = new Dictionary<ulong, GuildConfig>();
        public static Dictionary<ulong, UserConfig> UserConfig { get; set; } = new Dictionary<ulong, UserConfig>();

        public static GuildConfig GetGuildConfig(ulong guildId)
        {
            if (GuildConfig != null)
            {
                if (GuildConfig.TryGetValue(guildId, out GuildConfig? cfg))
                {
                    return cfg;
                }
                else
                {
                    return new GuildConfig();
                }
            }
            if (Directory.Exists($"Data/Guilds/{guildId}"))
            {
                return JsonConvert.DeserializeObject<GuildConfig>(File.ReadAllText($"Data/Guilds/{guildId}/config.json"))!;
            }
            else
            {
                Directory.CreateDirectory($"Data/Guilds/{guildId}");
                File.WriteAllText($"Data/Guilds/{guildId}/config.json", JsonConvert.SerializeObject(new GuildConfig(), Formatting.Indented));
                return GetGuildConfig(guildId);
            }
        }

        public static void SaveGuildConfig(ulong guildId, GuildConfig config)
        {
            GuildConfig[guildId] = config;
        }

        public static UserConfig GetUserConfig(ulong userId)
        {
            if (UserConfig != null)
            {
                if (UserConfig.TryGetValue(userId, out UserConfig cfg))
                {
                    return cfg;
                }
                else
                {
                    return new UserConfig();
                }
            }
            if (Directory.Exists($"Data/Users/{userId}"))
            {
                return JsonConvert.DeserializeObject<UserConfig>(File.ReadAllText($"Data/Users/{userId}/config.json"));
            }
            else
            {
                Directory.CreateDirectory($"Data/Users/{userId}");
                File.WriteAllText($"Data/Users/{userId}/config.json", JsonConvert.SerializeObject(new UserConfig(), Formatting.Indented));
                return GetUserConfig(userId);
            }
        }

        public static void SaveUserConfig(ulong userId, UserConfig config)
        {
            UserConfig[userId] = config;
        }

        public static void SaveToFile()
        {
            foreach (var guild in GuildConfig)
            {
                if (Directory.Exists($"Data/Guilds/{guild.Key}"))
                {
                    File.WriteAllText($"Data/Guilds/{guild.Key}/config.json", JsonConvert.SerializeObject(guild.Value, Formatting.Indented));
                }
                else
                {
                    Directory.CreateDirectory($"Data/Guilds/{guild.Key}");
                    File.WriteAllText($"Data/Guilds/{guild.Key}/config.json", JsonConvert.SerializeObject(guild.Value, Formatting.Indented));
                }
            }
            foreach (var user in UserConfig)
            {
                if (Directory.Exists($"Data/Users/{user.Key}"))
                {
                    File.WriteAllText($"Data/Users/{user.Key}/config.json", JsonConvert.SerializeObject(user.Value, Formatting.Indented));
                }
                else
                {
                    Directory.CreateDirectory($"Data/Users/{user.Key}");
                    File.WriteAllText($"Data/Users/{user.Key}/config.json", JsonConvert.SerializeObject(user.Value, Formatting.Indented));
                }
            }
        }

        public static void LoadFromFile()
        {
            if (!Directory.Exists("Data/Guilds"))
            {
                Directory.CreateDirectory("Data/Guilds");
            }
            if (!Directory.Exists("Data/Users"))
            {
                Directory.CreateDirectory("Data/Users");
            }
            foreach (var guild in Directory.GetDirectories("Data/Guilds"))
            {
                if (File.Exists($"{guild}/config.json"))
                {
                    GuildConfig.Add(ulong.Parse(guild.Split(new char[2] { '/', '\\' })[^1]),
                    JsonConvert.DeserializeObject<GuildConfig>(File.ReadAllText($"{guild}/config.json"))!);
                }
            }
            foreach (var user in Directory.GetDirectories("Data/Users"))
            {
                if (File.Exists($"{user}/config.json"))
                {
                    UserConfig.Add(ulong.Parse(user.Split(new char[2] { '/', '\\' })[^1]),
                    JsonConvert.DeserializeObject<UserConfig>(File.ReadAllText($"{user}/config.json")));
                }
            }
        }
    }
}
