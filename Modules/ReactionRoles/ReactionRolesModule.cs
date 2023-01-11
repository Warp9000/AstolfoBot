using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace AstolfoBot.Modules.ReactionRoles
{
    [Group("reaction-roles", "Commands for managing reaction roles")]
    public sealed class ReactionRolesModule : InteractionModuleBase<SocketInteractionContext>
    {
        ReactionRolesModule()
        {
            Main.Client.SelectMenuExecuted += Client_SelectMenuExecuted;
        }
        private async Task Client_SelectMenuExecuted(SocketInteraction interaction)
        {
            await RespondAsync(JsonConvert.SerializeObject(interaction));
        }
        private static readonly Dictionary<ulong, List<GuildEmote>> Emotes = new();
        private static readonly Dictionary<ulong, ReactionRoleMessage> ReactionRoleMessages = new();

        [SlashCommand("help", "Shows help for the reaction roles module")]
        public async Task HelpAsync()
        {
            Emotes.Add(Context.Guild.Id, Context.Guild.Emotes.ToList());
            var embed = new EmbedBuilder()
                .WithTitle("Reaction Roles")
                .WithDescription(
                    "Use `/reaction-roles add` to add a reaction role\n" +
                    "Use `/reaction-roles remove` to remove a reaction role\n" +
                    "Use `/reaction-roles list` to list all reaction roles\n" +
                    "Use `/reaction-roles edit` to edit embed related things\n" +
                    "Use `/reaction-roles clear` to clear all reaction roles\n" +
                    "Use `/reaction-roles cancel` to stop making the reaction role\n" +
                    "Use `/reaction-roles send` to send the message\n")
                .WithColor(Color.Blue)
                .WithFooter("Reaction Roles");

            await RespondAsync(embed: embed.Build());
        }

        [SlashCommand("create", "Creates a reaction role message")]
        public async Task CreateAsync()
        {
            if (ReactionRoleMessages.ContainsKey(Context.Guild.Id))
            {
                await RespondAsync("You already have a reaction role message in progress");
                return;
            }
            var embed = new EmbedBuilder()
                .WithTitle("Reaction Roles")
                .WithDescription("React to this message to get a role")
                .WithColor(Color.Blue)
                .WithFooter("Reaction Roles");
            var message = await Context.Channel.SendMessageAsync(embed: embed.Build());
            ReactionRoleMessages.Add(Context.Guild.Id, new ReactionRoleMessage() { Message = message, Embed = embed });
            await RespondAsync("Created reaction role message");
        }

        [SlashCommand("add", "Adds a reaction role")]
        public async Task AddAsync(
            [Summary("role", "The role to give")] SocketRole role)
        {
            if (!ReactionRoleMessages.TryGetValue(Context.Guild.Id, out var reactionRoleMessage))
            {
                await RespondAsync("You don't have a reaction role message in progress");
                return;
            }
            reactionRoleMessage.ReactionRoles.Add(role);
            var componentBuilder = new ComponentBuilder();
            var selectMenuBuilder = new SelectMenuBuilder()
                .WithCustomId("reaction-roles")
                .WithPlaceholder("Select a reaction role");
            foreach (var item in reactionRoleMessage.ReactionRoles)
            {
                selectMenuBuilder.AddOption(item.Name, item.Id.ToString(), "desc");
            }
            componentBuilder.WithSelectMenu(selectMenuBuilder);
            await reactionRoleMessage.Message.ModifyAsync(x =>
            {
                x.Embed = reactionRoleMessage.Embed.Build();
                x.Components = componentBuilder.Build();
            });
            await RespondAsync("Added reaction role");
        }
        private class ReactionRoleMessage
        {
            public Discord.Rest.RestUserMessage Message { get; set; } = null!;
            public List<IRole> ReactionRoles { get; set; } = new List<IRole>();
            public EmbedBuilder Embed { get; set; } = new EmbedBuilder();
        }
    }
}