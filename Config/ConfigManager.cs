using Newtonsoft.Json;

namespace AstolfoBot.Config
{
    public static class ConfigManager
    {
        public static Dictionary<ulong, GuildConfig> GuildConfig
        {
            get; set;
        } = new Dictionary<ulong, GuildConfig>();

        public static GuildConfig GetGuildConfig(ulong guildId)
        {
            if (GuildConfig != null)
            {
                if (GuildConfig.TryGetValue(guildId, out GuildConfig cfg))
                {
                    return cfg;
                }
                else
                {
                    GuildConfig.Add(guildId, new GuildConfig());
                    return GetGuildConfig(guildId);
                }
            }
            if (Directory.Exists($"Data/Guilds/{guildId}"))
            {
                return JsonConvert
                    .DeserializeObject<GuildConfig>(File
                        .ReadAllText($"Data/Guilds/{guildId}/config.json"));
            }
            else
            {
                Directory.CreateDirectory($"Data/Guilds/{guildId}");
                File
                    .WriteAllText($"Data/Guilds/{guildId}/config.json",
                    JsonConvert
                        .SerializeObject(new GuildConfig(),
                        Formatting.Indented));
                return GetGuildConfig(guildId);
            }
        }

        public static void SaveGuildConfig(ulong guildId, GuildConfig config)
        {
            GuildConfig[guildId] = config;
        }

        public static void SaveToFile()
        {
            foreach (var guild in GuildConfig)
            {
                if (Directory.Exists($"Data/Guilds/{guild.Key}"))
                {
                    File
                        .WriteAllText($"Data/Guilds/{guild.Key}/config.json",
                        JsonConvert
                            .SerializeObject(guild.Value, Formatting.Indented));
                }
                else
                {
                    Directory.CreateDirectory($"Data/Guilds/{guild.Key}");
                    File
                        .WriteAllText($"Data/Guilds/{guild.Key}/config.json",
                        JsonConvert
                            .SerializeObject(guild.Value, Formatting.Indented));
                }
            }
        }

        public static void LoadFromFile()
        {
            if (!Directory.Exists("Data/Guilds"))
            {
                Directory.CreateDirectory("Data/Guilds");
            }
            foreach (var guild in Directory.GetDirectories("Data/Guilds"))
            {
                if (File.Exists($"{guild}/config.json"))
                {
                    GuildConfig
                        .Add(ulong.Parse(guild.Split(new char[2] { '/', '\\' })[^1]),
                        JsonConvert
                            .DeserializeObject<GuildConfig>(File
                                .ReadAllText($"{guild}/config.json")));
                }
            }
        }
    }
}
