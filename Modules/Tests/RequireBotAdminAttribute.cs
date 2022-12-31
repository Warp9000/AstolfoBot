using Newtonsoft.Json;
using Discord.Interactions;
using Discord;

namespace AstolfoBot.Modules.Tests
{
    public class RequireBotAdminAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
        {
            if (!File.Exists("botadmins.json"))
            {
                File.WriteAllText("botadmins.json", "[]");
            }
            List<ulong> testers = JsonConvert.DeserializeObject<List<ulong>>(File.ReadAllText("botadmins.json")) ?? new List<ulong>();
            if (testers.Contains(context.User.Id))
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError("You are not a bot admin"));
            }
        }
    }
}