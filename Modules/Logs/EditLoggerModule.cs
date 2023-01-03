using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;

namespace AstolfoBot.Modules.Logs
{
    public class EditLoggerModule : InteractionModuleBase<SocketInteractionContext>
    {
        public EditLoggerModule()
        {
            Logger.Debug("EditLoggerModule loaded", this);
            Main.Client.MessageUpdated += OnMessageUpdated;
        }
        public Task OnMessageUpdated(Cacheable<IMessage, ulong> message, SocketMessage newMessage, ISocketMessageChannel channel)
        {
            GuildConfig cfg = new();
            if(channel is IGuildChannel guildChannel)
            {
                cfg = guildChannel.Guild.GetConfig();  
            }
            else
            {
                Logger.Debug("Channel is null", this);
                return Task.CompletedTask;
            }
            if (cfg.LogChannel is null)
            {
                Logger.Debug("LogChannel is null", this);
                return Task.CompletedTask;
            }
            if (message.HasValue && message.Value.Content != newMessage.Content)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Message edited")
                    .WithAuthor(message.Value.Author)
                    .AddField("Before", message.Value.Content)
                    .AddField("After", newMessage.Content)
                    .WithColor(Color.Blue)
                    .WithCurrentTimestamp()
                    .Build();
                cfg.LogChannel.SendMessageAsync(embed: embed);
            }
            return Task.CompletedTask;
        }
    }
}