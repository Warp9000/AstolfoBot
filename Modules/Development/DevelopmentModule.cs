using System.Diagnostics;
using System.Linq.Expressions;
using AstolfoBot.Config;
using AstolfoBot.Modules.Tests;
using Discord;
using Discord.Interactions;

namespace AstolfoBot.Modules.Development
{
    [Group("dev", "Development")]
    public class DevelopmentModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("configvalue", "Gets the value of a config key")]
        [RequireBotAdmin]
        public async Task GetConfigValue(string key)
        {
            var cfg = Context.Guild.GetConfig();
            var type = cfg.GetType();
            var prop = type.GetProperty(key);
            if (prop is null)
            {
                await RespondAsync("Property not found");
                return;
            }
            var value = prop.GetValue(cfg);
            if (value is null)
            {
                await RespondAsync("Property is null");
                return;
            }
            await RespondAsync(value.ToString());
        }

        [SlashCommand("unregistercommands", "Unregisters all commands")]
        [RequireBotAdmin]
        public async Task UnregisterCommands()
        {
            foreach (var guild in Context.Client.Guilds)
            {
                await guild.DeleteApplicationCommandsAsync();
            }
            await RespondAsync("Unregistered all commands");
        }

        [SlashCommand("exit", "Unregisters all commands and exits the program")]
        [RequireBotAdmin]
        public async Task Exit()
        {
            await DeferAsync();
            foreach (var guild in Context.Client.Guilds)
            {
                await guild.DeleteApplicationCommandsAsync();
            }
            await FollowupAsync("Unregistered all commands");
            await FollowupAsync("Exiting...");
            await Main.StopAsync();
            Environment.Exit(0);
        }
    }
}
