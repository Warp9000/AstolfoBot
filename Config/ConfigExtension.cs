using Discord;
using Discord.WebSocket;

namespace AstolfoBot.Config
{
    public static class ConfigExtension
    {
        public static GuildConfig GetConfig(this SocketGuild guild) => ConfigManager.GetGuildConfig(guild.Id);
        public static void SaveConfig(this SocketGuild guild, GuildConfig config) => ConfigManager.SaveGuildConfig(guild.Id, config);
        public static void SaveConfig(this GuildConfig config, SocketGuild guild) => ConfigManager.SaveGuildConfig(guild.Id, config);
    }
}