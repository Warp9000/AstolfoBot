using Discord;
using Discord.Interactions;
using Newtonsoft.Json;

namespace AstolfoBot.Modules.Tests
{
    public class TesterModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("addtester", "Adds a tester")]
        [Tests.RequireBotAdmin]
        public async Task AddTesterCommand([Summary("user", "The user to add as a tester")] IUser user)
        {
            try
            {
                if (!File.Exists("testers.json"))
                {
                    File.WriteAllText("testers.json", "[]");
                }
                List<ulong> testers = JsonConvert.DeserializeObject<List<ulong>>(File.ReadAllText("testers.json")) ?? new List<ulong>();
                if (testers.Contains(user.Id))
                {
                    await RespondAsync("This user is already a tester");
                }
                else
                {
                    testers.Add(user.Id);
                    File.WriteAllText("testers.json", JsonConvert.SerializeObject(testers, Formatting.Indented));
                    await RespondAsync($"Added user {user.Username}#{user.Discriminator} ({user.Id}) as a tester");
                }
            }
            catch (Exception e)
            {
                await RespondAsync($"```json\n{JsonConvert.SerializeObject(e, Formatting.Indented)}\n```");
            }
        }
        [SlashCommand("removetester", "Removes a tester")]
        [Tests.RequireBotAdmin]
        public async Task RemoveTesterCommand([Summary("user", "The user to remove as a tester")] IUser user)
        {
            try
            {
                if (!File.Exists("testers.json"))
                {
                    File.WriteAllText("testers.json", "[]");
                }
                List<ulong> testers = JsonConvert.DeserializeObject<List<ulong>>(File.ReadAllText("testers.json")) ?? new List<ulong>();
                if (testers.Contains(user.Id))
                {
                    testers.Remove(user.Id);
                    File.WriteAllText("testers.json", JsonConvert.SerializeObject(testers, Formatting.Indented));
                    await RespondAsync($"Removed user {user.Username}#{user.Discriminator} ({user.Id}) as a tester");
                }
                else
                {
                    await RespondAsync("This user is not a tester");
                }
            }
            catch (Exception e)
            {
                await RespondAsync($"```json\n{JsonConvert.SerializeObject(e, Formatting.Indented)}\n```");
            }
        }
    }
}