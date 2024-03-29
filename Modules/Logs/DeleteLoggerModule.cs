using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using AstolfoBot.Config;
using Newtonsoft.Json;

namespace AstolfoBot.Modules.Logs
{
    public class DeleteLoggerModule : InteractionModuleBase<SocketInteractionContext>
    {
        public DeleteLoggerModule()
        {
            Logger.Debug("DeleteLoggerModule loaded", this);
            Main.Client.MessageDeleted += OnMessageDeleted;
            Main.Client.MessagesBulkDeleted += OnMessagesBulkDeleted;
        }
        public async Task OnMessageDeleted(Cacheable<IMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel)
        {
            try
            {
                if (channel.HasValue && channel.Value is IGuildChannel guildChannel)
                {
                    GuildConfig cfg = guildChannel.Guild.GetConfig();
                    if (message.HasValue)
                    {
                        if (message.Value.Author.IsBot)
                        {
                            Logger.Debug("Message is from a bot", this);
                            return;
                        }
                        if (cfg.LogChannel is not null)
                        {
                            var embed = new EmbedBuilder()
                                .WithTitle("Message deleted in <#" + message.Value.Channel.Id + ">")
                                .WithAuthor(message.Value.Author)
                                .WithDescription(message.Value.Content)
                                .WithColor(Color.Red)
                                .WithCurrentTimestamp()
                                .Build();
                            await cfg.LogChannel.SendMessageAsync(embed: embed);
                        }
                        else
                        {
                            Logger.Debug("LogChannel is null", this);
                        }
                        FileLogManager.WriteDeletedMessages(guildChannel.GuildId, channel.Id, new List<JsonMessage>() { JsonMessage.FromIMessage(message.Value) });
                    }
                }
                else
                {
                    Logger.Debug("Channel is null", this);
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.Critical(ex.Message, this, ex);
            }
        }
        public async Task OnMessagesBulkDeleted(IReadOnlyCollection<Cacheable<IMessage, ulong>> messages, Cacheable<IMessageChannel, ulong> channel)
        {
            if (channel.HasValue && channel.Value is IGuildChannel guildChannel)
            {
                GuildConfig cfg = guildChannel.Guild.GetConfig();
                if (messages.Count > 0)
                {
                    string messageContent = "yyyy-MM-dd HH:mm:ss | Author: Content\n--------------------------------------------------\n";
                    var msgs = messages.ToList();
                    msgs.RemoveAll(x => !x.HasValue || x.Value.Author.IsBot);
                    msgs.Sort((x, y) => x.Value.Timestamp.CompareTo(y.Value.Timestamp));
                    if (!msgs.Any(x => !string.IsNullOrEmpty(x.Value.Content)))
                    {
                        return;
                    }
                    foreach (var message in msgs)
                    {
                        if (message.HasValue)
                        {
                            messageContent += message.Value.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + " | " +
                            $"{message.Value.Author.Username}#{message.Value.Author.Discriminator} ({message.Value.Author.Id}): " +
                            message.Value.Content + "\n";
                        }
                    }
                    File.WriteAllText("bulkDelete.txt", messageContent);
                    if (cfg.LogChannel is not null)
                    {
                        var embed = new EmbedBuilder()
                        .WithTitle("Bulk delete in <#" + channel.Value.Id + $"> ({msgs.Count} messages)")
                        .WithColor(Color.Red)
                        .WithCurrentTimestamp()
                        .Build();
                        await cfg.LogChannel.SendFileAsync("bulkDelete.txt", embed: embed);
                    }
                    else
                    {
                        Logger.Debug("LogChannel is null", this);
                    }
                    FileLogManager.WriteDeletedMessages(guildChannel.GuildId, channel.Id, msgs.Select(x => JsonMessage.FromIMessage(x.Value)).ToList());
                    File.Delete("bulkDelete.txt");

                }
            }
            else
            {
                Logger.Debug("Channel is null", this);
                return;
            }

        }
    }
}