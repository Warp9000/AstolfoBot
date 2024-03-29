using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace AstolfoBot
{
    public class CommandHandler
    {
        public readonly DiscordSocketClient Client;
        public InteractionService InteractionService = null!;

        public CommandHandler(Discord.WebSocket.DiscordSocketClient client)
        {
            Client = client;
            InteractionService = new(Client);
        }

        public async Task InstallCommandsAsync()
        {
            Logger.Verbose("Starting...", this);
            Client.JoinedGuild += async (guild) =>
            {
                await InteractionService.RegisterCommandsToGuildAsync(guild.Id, true);
                Logger.Debug("Added commands to " + guild.Id, this);
            };
            Client.Ready += () =>
            {
                Task.Run(RegisterCommands);
                Logger.Debug("Added RegisterCommands to ready handler", this);
                return Task.CompletedTask;
            };
            InteractionService.SlashCommandExecuted += SlashCommandExecuted;
            await InteractionService.AddModulesAsync(assembly: System.Reflection.Assembly.GetExecutingAssembly(),
                                            services: null);

            Logger.Debug(InteractionService.Modules.Count + " modules loaded:", this);
            Logger.Debug(InteractionService.SlashCommands.Count + " commands loaded:", this);
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
                // Logger.Debug($"Executed command result: {result.IsSuccess}: {result.Error}: {result.ErrorReason}", this);
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
                        var admins = JsonConvert.DeserializeObject<ulong[]>(File.ReadAllText("botadmins.json")) ?? Array.Empty<ulong>();
                        var testers = JsonConvert.DeserializeObject<ulong[]>(File.ReadAllText("testers.json")) ?? Array.Empty<ulong>();
                        if (admins.Contains(context.User.Id) || testers.Contains(context.User.Id))
                        {
                            await context.Interaction.RespondAsync(
                                $"Command exception: {result.ErrorReason}\nHeres the entire thing cause youre a tester\n" +
                                $"```json\n{JsonConvert.SerializeObject(result, Formatting.Indented)}\n```");
                        }
                        else
                        {
                            await context.Interaction.RespondAsync($"Command exception: {result.ErrorReason}");
                        }
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