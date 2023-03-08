using System.Diagnostics;
using System.Linq.Expressions;
using AstolfoBot.Config;
using AstolfoBot.Modules.Tests;
using Discord;
using Discord.Interactions;
using System.Text.RegularExpressions;

namespace AstolfoBot.Modules.Development
{
    [Group("dev", "Development")]
    public class DevelopmentModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("cmd", "yuh")]
        [RequireBotAdmin]
        public async Task Cmd(string command, string? args = null)
        {
            var method = GetType().GetMethod(command);
            if (method == null)
            {
                await RespondAsync("Command not found");
                return;
            }
            var arguments = new List<object>();
            var str = args?.Split(' ');
            if (str != null)
                foreach (var item in str)
                {
                    if (item.StartsWith('"') && item.EndsWith('"'))
                    {
                        arguments.Add(item[1..^1]);
                    }
                    else if (int.TryParse(item, out var i))
                    {
                        arguments.Add(i);
                    }
                    else if (bool.TryParse(item, out var b))
                    {
                        arguments.Add(b);
                    }
                    else if (float.TryParse(item, out var f))
                    {
                        arguments.Add(f);
                    }
                    else
                    {
                        arguments.Add(item);
                    }
                }
            try
            {
                var o = method!.Invoke(this, arguments.ToArray());
                if (o is Task t)
                {
                    await t;
                }
                if (Context.Interaction.HasResponded)
                {
                    return;
                }
                if (o != null && o.GetType() == typeof(string[]))
                {
                    o = string.Join("\n", (string[])o);
                }
                await RespondAsync(o?.ToString() ?? "Command executed", ephemeral: true);
            }
            catch (Exception e)
            {
                if (!Context.Interaction.HasResponded)
                    await RespondAsync(e.Message, ephemeral: true);
                else
                    await FollowupAsync(e.Message, ephemeral: true);
            }
        }
        public async Task<string> UnregisterCommands()
        {
            foreach (var guild in Context.Client.Guilds)
            {
                await guild.DeleteApplicationCommandsAsync();
            }
            return "Unregistered all commands";
        }
        public async Task Exit()
        {
            await DeferAsync(ephemeral: true);
            foreach (var guild in Context.Client.Guilds)
            {
                await guild.DeleteApplicationCommandsAsync();
            }
            await FollowupAsync("Unregistered all commands", ephemeral: true);
            await Main.StopAsync();
            Environment.Exit(0);
        }
        public string[] Help()
        {
            var methods = GetType().GetMethods();
            var list = new List<string>();
            foreach (var method in methods)
            {
                var name = method.Name;
                var args = method.GetParameters().ToArray();
                var str = $"{name}({string.Join(", ", args.Select(x => x.ParameterType.Name + " " + x.Name))})";
                list.Add(str);
            }
            return list.ToArray();
        }
        public string[] GetGuilds()
        {
            var list = new List<string>();
            foreach (var guild in Context.Client.Guilds)
            {
                list.Add(guild.Name + " " + guild.Id);
            }
            return list.ToArray();
        }
        public async Task<string> GetGuildInvite(ulong guildId)
        {
            var guild = Context.Client.GetGuild(guildId);
            var i = await guild.GetInvitesAsync();
            var invites = i.ToList();
            if (invites == null || invites.Count == 0)
            {
                return "No invites found";
            }
            return invites.MaxBy(x => x.Uses)!.Code;
        }
    }
}
