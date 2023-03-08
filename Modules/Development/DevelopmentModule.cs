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
                await RespondAsync("Command not found", ephemeral: true);
                return;
            }
            var arguments = new List<object>();
            var str = args?.Split(' ');
            if (str != null)
                foreach (var item in str)
                {
                    if (item.ToLower().StartsWith('u'))
                    {
                        if (uint.TryParse(item[1..], out var ui))
                            arguments.Add(ui);
                        else if (ulong.TryParse(item[1..], out var ul))
                            arguments.Add(ul);
                        else
                            break;
                        continue;

                    }
                    if (item.StartsWith('"') && item.EndsWith('"'))
                    {
                        arguments.Add(item[1..^1]);
                    }
                    else if (int.TryParse(item, out var i))
                    {
                        arguments.Add(i);
                    }
                    else if (long.TryParse(item, out var l))
                    {
                        arguments.Add(l);
                    }
                    else if (bool.TryParse(item, out var b))
                    {
                        arguments.Add(b);
                    }
                    else if (float.TryParse(item, out var f))
                    {
                        arguments.Add(f);
                    }
                    else if (double.TryParse(item, out var d))
                    {
                        arguments.Add(d);
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
                    o = t.GetType().GetProperty("Result")?.GetValue(t);
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
                    await RespondAsync(e.Message + "\n" + e.StackTrace, ephemeral: true);
                else
                    await FollowupAsync(e.Message + "\n" + e.StackTrace, ephemeral: true);
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
            var invite = invites.MaxBy(x => x.Uses)!;
            return $"{invite.Url} {invite.Uses} uses";
        }
    }
}
