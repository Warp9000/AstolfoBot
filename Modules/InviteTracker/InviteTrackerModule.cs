using Newtonsoft.Json;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using AstolfoBot.Config;

namespace AstolfoBot.Modules.InviteTracker
{
    public sealed class InviteTrackerModule : InteractionModuleBase<SocketInteractionContext>
    {
        static readonly Dictionary<ulong, IInviteMetadata[]> Invites = new();
        InviteTrackerModule()
        {
            Main.Client.Ready += () =>
            {
                foreach (var guild in Main.Client.Guilds)
                {
                    var invites = guild.GetInvitesAsync().Result;
                    Invites.Add(guild.Id, invites.ToArray());
                }
                return Task.CompletedTask;
            };
            Main.Client.UserJoined += OnUserJoined;
            Main.Client.InviteCreated += OnInviteCreated;
        }
        public class Invite
        {
            public string Url { get; set; } = "";
            public int? Uses { get; set; }
        }
        [SlashCommand("invites", "Gets the invites of the guild")]
        [Tests.RequireTester]
        public async Task InvitesCommand()
        {
            try
            {
                var invites = await Context.Guild.GetInvitesAsync();

                File.WriteAllText("invites.json", JsonConvert.SerializeObject(invites, Formatting.Indented));
                await RespondWithFileAsync("invites.json");
                File.Delete("invites.json");
            }
            catch (Exception e)
            {
                await RespondAsync(JsonConvert.SerializeObject(e, Formatting.Indented));
            }
        }
        public async Task OnUserJoined(SocketGuildUser socketGuildUser)
        {
            var oldInvites = Invites[socketGuildUser.Guild.Id];
            var newInvites = await socketGuildUser.Guild.GetInvitesAsync();
            foreach (var inv in oldInvites)
            {
                if (newInvites.FirstOrDefault(x => x.Code == inv.Code)?.Uses > inv.Uses)
                {
                    var cfg = Context.Guild.GetConfig();
                    if (cfg.LogChannel != null)
                        await cfg.LogChannel.SendMessageAsync($"User {socketGuildUser.Mention} was invited by {inv.Inviter} using invite `{inv.Url}`");
                }
            }
        }
        public static async Task OnInviteCreated(SocketInvite socketInvite)
        {
            var invites = await socketInvite.Guild.GetInvitesAsync();
            Invites[socketInvite.Guild.Id] = invites.ToArray();
        }
    }
}