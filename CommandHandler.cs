using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace AstolfoBot
{
    public class CommandHandler
    {
        public readonly DiscordSocketClient Client;
        public static InteractionService InteractionService = null!;

        public CommandHandler(Discord.WebSocket.DiscordSocketClient client)
        {
            Client = client;
            InteractionService = new(Client);
        }

        public async Task InstallCommandsAsync()
        {
            Logger.Verbose("Starting...", this);
            Client.Ready += () =>
            {
                Task.Run(RegisterCommands);
                Logger.Debug("Added RegisterCommands to ready handler", this);
                return Task.CompletedTask;
            };
            InteractionService.SlashCommandExecuted += SlashCommandExecuted;
            await InteractionService.AddModulesAsync(assembly: System.Reflection.Assembly.GetExecutingAssembly(),
                                            services: null);

            Logger.Debug(InteractionService.Modules.Count() + " modules loaded:", this);
            foreach (var module in InteractionService.Modules)
            {
                Logger.Debug(module.Name, this);
            }
            Logger.Debug(InteractionService.SlashCommands.Count + " commands loaded:", this);
            foreach (var command in InteractionService.SlashCommands)
            {
                Logger.Debug(command.Name, this);
            }
            Client.InteractionCreated += HandleInteraction;
        }

        private async Task RegisterCommands()
        {
            try
            {
                foreach (var guild in Client.Guilds)
                {
                    await InteractionService.RegisterCommandsToGuildAsync(guild.Id, true);
                    Logger.Debug("Added commands to " + guild.Id, this);
                }
            }
            catch (Exception ex)
            {
                Logger.Critical(ex.Message, this, ex);
            }
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(Client, interaction);
                var result = await InteractionService.ExecuteCommandAsync(context, null);
                Logger.Debug($"Executed command result: {result.IsSuccess}: {result.Error}: {result.ErrorReason}", this);
            }
            catch (Exception ex)
            {
                Logger.Critical(ex.Message, this, ex);
                if (interaction.Type is InteractionType.ApplicationCommand)
                    await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }

        private async Task SlashCommandExecuted(SlashCommandInfo slashCommandInfo, Discord.IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        await context.Interaction.RespondAsync($"Unmet Precondition: {result.ErrorReason}");
                        break;
                    case InteractionCommandError.UnknownCommand:
                        await context.Interaction.RespondAsync("Unknown command");
                        break;
                    case InteractionCommandError.BadArgs:
                        await context.Interaction.RespondAsync("Invalid number or arguments");
                        break;
                    case InteractionCommandError.Exception:
                        await context.Interaction.RespondAsync($"Command exception: {result.ErrorReason}");
                        break;
                    case InteractionCommandError.Unsuccessful:
                        await context.Interaction.RespondAsync("Command could not be executed");
                        break;
                    default:
                        await context.Interaction.RespondAsync(result.Error.ToString() + ": " + result.ErrorReason);
                        break;
                }
            }
        }
    }
}