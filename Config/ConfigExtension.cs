using Discord;
using Discord.WebSocket;

namespace AstolfoBot.Config
{
    public static class ConfigExtension
    {
        public static GuildConfig GetConfig(this IGuild guild) =>
            ConfigManager.GetGuildConfig(guild.Id);

        public static void SaveConfig(this IGuild guild, GuildConfig config) =>
            ConfigManager.SaveGuildConfig(guild.Id, config);

        public static void SaveConfig(this GuildConfig config, IGuild guild) =>
            ConfigManager.SaveGuildConfig(guild.Id, config);
    }
}
