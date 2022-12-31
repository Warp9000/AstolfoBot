using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;

namespace AstolfoBot.Modules.Logs
{
    public class LoggerModule : InteractionModuleBase<SocketInteractionContext>
    {
        public LoggerModule()
        {
            Logger.Debug("LoggerModule loaded", this);
        }
        [SlashCommand("logchannel", "Sets the channel to log to")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task LogChannelCommand([Summary("channel", "The channel to log to")] ITextChannel? channel = null)
        {
            var cfg = Context.Guild.GetConfig();
            if (channel is null)
            {
                await RespondAsync("The current log channel is " + cfg.LogChannel?.Mention ?? "not set");
                return;
            }
            cfg.LogChannel = channel;
            cfg.SaveConfig(Context.Guild);
            await RespondAsync("Set log channel to " + channel.Mention);
        }
        public static async Task LogToChannel(string title, SocketGuild guild, string description = "", string footer = "", string author = "", Color color = default)
        {
            var cfg = guild.GetConfig();
            if (cfg.LogChannel is null)
            {
                Logger.Warning("Log channel not set", "LoggerModule");
                return;
            }
            var eb = new EmbedBuilder()
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(color)
                .WithFooter(footer)
                .WithAuthor(author)
                .WithCurrentTimestamp();
            await cfg.LogChannel.SendMessageAsync(embed: eb.Build());
        }
        public static async Task LogToChannel(Embed embed, SocketGuild guild)
        {
            var cfg = guild.GetConfig();
            if (cfg.LogChannel is null)
            {
                Logger.Warning("Log channel not set", "LoggerModule");
                return;
            }
            await cfg.LogChannel.SendMessageAsync(embed: embed);
        }
    }
}
