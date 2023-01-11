using System.Text;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;

namespace AstolfoBot.Modules.Logs
{
    [Group("logs", "Logs")]
    public class LoggerModule : InteractionModuleBase<SocketInteractionContext>
    {
        public LoggerModule()
        {
            Logger.Debug("LoggerModule loaded", this);
        }
        [SlashCommand("logchannel", "Sets or gets the channel to log to")]
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
        [SlashCommand("getlogs", "Gets logs in the specified time frame")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task GetLogsCommand(
            [Summary("type","Specifies wether to get delted or editied messages")]LogType type,
            [Summary("channel", "The channel to get logs from")] ITextChannel? channel = null,
            [Summary("start", "The start of the time frame")] DateTime? start = null,
            [Summary("end", "The end of the time frame")] DateTime? end = null)
        {
            try
            {
            start ??= DateTime.Now.AddDays(-1);
            end ??= DateTime.Now;
            channel ??= Context.Channel as ITextChannel;
            if (channel is null)
            {
                await RespondAsync("Channel not found");
                return;
            }
            if (start > end)
            {
                await RespondAsync("Start date must be before end date");
                return;
            }
            if (type == LogType.Deleted)
            {
                var messages = FileLogManager.GetDeletedMessages(Context.Guild.Id, channel.Id, (start.Value, end.Value));
                if (messages.Count == 0)
                {
                    await RespondAsync("No messages found");
                    return;
                }
                string msg = "Messages deleted in " + channel.Name + " (" + channel.Id + ") between " +
                start.Value.ToString("dd/MM/yyyy HH:mm:ss") + " and " + end.Value.ToString("dd/MM/yyyy HH:mm:ss") + "\n\n";
                foreach (var message in messages)
                {
                    msg += $"{message.Timestamp:dd/MM/yyyy HH:mm:ss} | {message.AuthorUsername}#{message.AuthorDiscriminator} " +
                    $"({message.AuthorId}): {message.Content}\n";
                }
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(msg));
                await RespondWithFileAsync(stream, "logs.txt");
            }
            else if (type == LogType.Edited)
            {
                var messages = FileLogManager.GetEditedMessages(Context.Guild.Id, channel.Id, (start.Value, end.Value));
                if (messages.Count == 0)
                {
                    await RespondAsync("No messages found");
                    return;
                }
                string msg = "Messages edited in #" + channel.Name + " (" + channel.Id + ") between " +
                start.Value.ToString("dd/MM/yyyy HH:mm:ss") + " and " + end.Value.ToString("dd/MM/yyyy HH:mm:ss") + "\n\n";
                foreach (var message in messages)
                {
                    msg += $"{message.Item1.Timestamp:dd/MM/yyyy HH:mm:ss} | "+
                    $"{message.Item1.AuthorUsername}#{message.Item1.AuthorDiscriminator} " + $"({message.Item1.AuthorId})\n"+
                    $"\tBefore: {message.Item1.Content}\n\tAfter: {message.Item2.Content}\n";
                }
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(msg));
                await RespondWithFileAsync(stream, "logs.txt");
            
            }
            }
            catch (Exception e)
            {
                Logger.Error("Error getting logs", "LoggerModule", e);
            }
        }

        public enum LogType
        {
            Deleted = 0,
            Edited = 1
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
