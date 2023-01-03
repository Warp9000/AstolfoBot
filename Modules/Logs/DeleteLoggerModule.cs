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
                GuildConfig cfg = new();
                if(channel.HasValue && channel.Value is IGuildChannel guildChannel)
                {
                    cfg = guildChannel.Guild.GetConfig();  
                }
                else
                {
                    Logger.Debug("Channel is null", this);
                    return;
                }
                if (cfg.LogChannel is null)
                {
                    Logger.Debug("LogChannel is null", this);
                    return;
                }
                if (message.HasValue)
                {
                    var embed = new EmbedBuilder()
                        .WithTitle("Message deleted")
                        .WithAuthor(message.Value.Author)
                        .WithDescription(message.Value.Content)
                        .WithColor(Color.Red)
                        .WithCurrentTimestamp()
                        .Build();
                    await cfg.LogChannel.SendMessageAsync(embed: embed);
                }
            }
            catch (Exception ex)
            {
                Logger.Critical(ex.Message, this, ex);
            }
        }
        public async Task OnMessagesBulkDeleted(IReadOnlyCollection<Cacheable<IMessage, ulong>> messages, Cacheable<IMessageChannel, ulong> channel)
        {
            GuildConfig cfg = new();
            if(channel.HasValue && channel.Value is IGuildChannel guildChannel)
            {
                cfg = guildChannel.Guild.GetConfig();
            }
            else
            {
                Logger.Debug("Channel is null", this);
                return;
            }
            if (cfg.LogChannel is null)
            {
                Logger.Debug("LogChannel is null", this);
                return;
            }
            if (messages.Count > 0)
            {
                List<IUser> authors = new();
                foreach (var message in messages)
                {
                    if (message.HasValue)
                    {
                        if (authors.Contains(message.Value.Author))
                        {
                            continue;
                        }
                        authors.Add(message.Value.Author);
                    }
                }
                bool multipleAuthors = authors.Count > 1;

                string messageContent = "yyyy-MM-dd HH:mm:ss | Author: Content\n--------------------------------------------------\n";
                var msgs = messages.ToList();
                msgs.Sort((x, y) => x.Value.Timestamp.CompareTo(y.Value.Timestamp));
                foreach (var message in msgs)
                {
                    if (message.HasValue)
                    {
                        messageContent += message.Value.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + " | " +
                        $"{authors.First().Username}#{authors.First().Discriminator} ({authors.First().Id}): " +
                        message.Value.Content + "\n";
                    }
                }

                var embed = new EmbedBuilder()
                    .WithTitle("Bulk delete")
                    .WithAuthor(new EmbedAuthorBuilder()
                        .WithName(multipleAuthors ? "Multiple authors" :
                        $"{authors.First().Username}#{authors.First().Discriminator} ({authors.First().Id})")
                        .WithIconUrl(multipleAuthors ? null : authors.First().GetAvatarUrl()))
                    .WithColor(Color.Red)
                    .WithCurrentTimestamp()
                    .Build();
                File.WriteAllText("bulkDelete.txt", messageContent);
                await cfg.LogChannel.SendFileAsync("bulkDelete.txt", embed: embed);
                File.Delete("bulkDelete.txt");
            }
            return;
        }
    }
}