using AstolfoBot.Config;
using AstolfoBot.Modules.Tests;
using Discord;
using Discord.Interactions;

namespace AstolfoBot.Modules.Development
{
    [Group("dev", "Development")]
    public partial class
    DevelopmentModule
    : InteractionModuleBase<SocketInteractionContext>
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
    }
}
