using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace AstolfoBot.Modules.Moderation
{
    public partial class ModerationModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("purge", "Purges messages")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task PurgeMessagesAsync([Summary("amount", "The amount of messages to purge")] int amount)
        {
            var messages = await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
            await (Context.Channel as SocketTextChannel)!.DeleteMessagesAsync(messages);
            var embed = new EmbedBuilder()
                .WithTitle("Messages purged")
                .WithDescription($"Amount: {amount}")
                .WithColor(Color.Red)
                .WithCurrentTimestamp()
                .Build();
            await Logs.LoggerModule.LogToChannel(embed, Context.Guild);
            await RespondAsync(embed: embed);
        }
    }
}
