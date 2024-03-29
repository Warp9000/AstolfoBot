using System.Threading;
using System.ComponentModel;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;
using AstolfoBot.Modules.Tests;

namespace AstolfoBot.Modules.Counting
{
    public class CountingModule : InteractionModuleBase<SocketInteractionContext>
    {
        CountingModule()
        {
            Main.Client.MessageReceived += OnMessageReceived;
            Main.Client.MessageDeleted += OnMessageDeleted;
        }

        private async Task OnMessageReceived(SocketMessage message)
        {
            try
            {
                if (message.Channel is not ITextChannel channel)
                    return;

                var config = ((ITextChannel)message.Channel).Guild.GetConfig();
                if (config.Counting.CountingChannel == null)
                    return;

                if (config.Counting.CountingChannel.Id != channel.Id)
                    return;

                if (message.Author.IsBot)
                    return;

                if (config.Counting.LastUser != null && config.Counting.LastUser.Id == message.Author.Id)
                {
                    await message.DeleteAsync();
                    return;
                }

                if (!message.Content.Contains((config.Counting.CurrentNumber + 1).ToString()))
                {
                    await message.DeleteAsync();
                    return;
                }

                config.Counting.CurrentNumber++;
                config.Counting.LastUser = (IGuildUser)message.Author;
                config.SaveConfig(((ITextChannel)message.Channel).Guild);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, this, e);
            }
        }
        private async Task OnMessageDeleted(Cacheable<IMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel)
        {
            try
            {
                if (channel.Value is not ITextChannel textChannel)
                    return;

                var config = textChannel.Guild.GetConfig();
                if (config.Counting.CountingChannel == null)
                    return;

                if (config.Counting.CountingChannel.Id != textChannel.Id)
                    return;

                if (message.Value.Author.IsBot)
                    return;

                if (config.Counting.LastUser == null)
                    return;

                if (config.Counting.LastUser.Id == message.Value.Author.Id)
                {
                    var chnl = channel.Value as ITextChannel;
                    await chnl!.SendMessageAsync($"{message.Value.Author.Mention} deleted their message. The next number is {config.Counting.CurrentNumber + 1}");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, this, e);
            }
        }

        [Group("counting", "Commands for the counting game")]
        public class CountingGroup : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("channel", "Set the counting channel")]
            [RequireUserPermission(GuildPermission.ManageChannels)]
            public async Task ChannelCommand([Description("The channel to set")] ITextChannel? channel = null)
            {
                var config = Context.Guild.GetConfig();
                config.Counting.CountingChannel = channel;
                config.SaveConfig(Context.Guild);

                await RespondAsync($"Counting channel set to {channel?.Mention}");
            }

            [SlashCommand("reset", "Reset the counting game")]
            [RequireUserPermission(GuildPermission.ManageChannels)]
            public async Task ResetCommand()
            {
                var config = Context.Guild.GetConfig();
                config.Counting.CurrentNumber = 0;
                config.Counting.LastUser = null;
                config.SaveConfig(Context.Guild);

                await RespondAsync("Counting game reset");
            }

            [SlashCommand("next", "Gets the next number to be counted")]
            public async Task CurrentCommand()
            {
                var config = Context.Guild.GetConfig();
                await RespondAsync($"Next number is {config.Counting.CurrentNumber + 1}");
            }

            [SlashCommand("set", "Set the current number")]
            [RequireBotAdmin]
            public async Task SetCommand([Description("The number to set")] int number)
            {
                var config = Context.Guild.GetConfig();
                config.Counting.CurrentNumber = number;
                config.SaveConfig(Context.Guild);

                await RespondAsync($"Current number set to {number}");
            }
        }
    }
}