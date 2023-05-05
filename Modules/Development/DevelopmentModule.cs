using System.Diagnostics;
using System.Linq.Expressions;
using AstolfoBot.Config;
using AstolfoBot.Modules.Tests;
using Discord;
using Discord.Interactions;
using System.Text.RegularExpressions;
using System.Reflection;

namespace AstolfoBot.Modules.Development
{
    [Group("dev", "Development")]
    public class DevelopmentModule : InteractionModuleBase<SocketInteractionContext>
    {
        private Dictionary<string, MethodInfo> Aliases = new Dictionary<string, MethodInfo>();
        DevelopmentModule()
        {
            foreach (var method in GetType().GetMethods())
            {
                if (method.GetCustomAttributes(typeof(DevCommandAttribute), false).Length == 0)
                {
                    continue;
                }
                var attr = method.GetCustomAttribute<DevCommandAttribute>();
                if (attr == null)
                {
                    continue;
                }
                foreach (var alias in attr.Aliases)
                {
                    Aliases.Add(alias, method);
                }
            }
        }

        [SlashCommand("cmd", "yuh")]
        [RequireBotAdmin]
        public async Task Cmd(string command, string? args = null)
        {
            var method = GetType().GetMethod(command) ?? Aliases.GetValueOrDefault(command);
            if (method == null || method.GetCustomAttributes(typeof(DevCommandAttribute), false).Length == 0)
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
                string oStr = o?.ToString() ?? "Command executed";
                if (oStr.Length < 2000)
                {
                    await RespondAsync(oStr, ephemeral: true);
                    return;
                }
                // respond with file
                var file = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(oStr));
                await RespondWithFileAsync(file, "output.txt", ephemeral: true);
            }
            catch (Exception e)
            {
                if (!Context.Interaction.HasResponded)
                    await RespondAsync(e.Message + "\n" + e.StackTrace, ephemeral: true);
                else
                    await FollowupAsync(e.Message + "\n" + e.StackTrace, ephemeral: true);
            }
        }
        [DevCommand]
        public string[] Help()
        {
            var methods = GetType().GetMethods().Where(x => x.GetCustomAttributes(typeof(DevCommandAttribute), false).Length > 0);
            var list = new List<string>();
            foreach (var method in methods)
            {
                var name = method.Name;
                var args = method.GetParameters().ToArray();
                string[]? aliases = method.GetCustomAttribute<DevCommandAttribute>()?.Aliases;
                aliases = aliases?.Length == 0 ? null : aliases;
                var str = $"{name}({string.Join(", ", args.Select(x => x.ParameterType.Name + " " + x.Name))})";
                if (aliases != null)
                    str += $" [{string.Join(", ", aliases)}]";
                list.Add(str);
            }
            return list.ToArray();
        }
        [DevCommand]
        public async Task<string> UnregisterCommands()
        {
            foreach (var guild in Context.Client.Guilds)
            {
                await guild.DeleteApplicationCommandsAsync();
            }
            return "Unregistered all commands";
        }
        [DevCommand]
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
        [DevCommand(new[] { "GetG", "GG" })]
        public string[] GetGuilds()
        {
            var list = new List<string>();
            foreach (var guild in Context.Client.Guilds)
            {
                list.Add(guild.Name + " " + guild.Id);
            }
            return list.ToArray();
        }
        [DevCommand(new[] { "GetGI", "GGI" })]
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
        [DevCommand(new[] { "CreateGI", "CGI" })]
        public async Task<string> CreateInvite(ulong guildId)
        {
            var guild = Context.Client.GetGuild(guildId);
            var invite = await guild.TextChannels.First().CreateInviteAsync(120, 1, false, true);
            return invite.Url;
        }
        [DevCommand(new[] { "FindU", "FU" })]
        public string[] FindUser(string name)
        {
            var users = Context.Client.Guilds.SelectMany(x => x.Users).Where(x => x.Username.ToLower().Contains(name.ToLower()));
            var list = new List<string>();
            foreach (var user in users)
            {
                list.Add($"{user.Username}#{user.Discriminator} ({user.Id}) in {user.Guild.Name} ({user.Guild.Id})");
            }
            return list.ToArray();
        }
        [DevCommand(new[] { "ListR", "LR" })]
        public string[] ListRoles(ulong guildId)
        {
            var guild = Context.Client.GetGuild(guildId);
            var list = new List<string>();
            foreach (var role in guild.Roles)
            {
                list.Add($"{role.Name} ({role.Id})");
            }
            return list.ToArray();
        }
        [DevCommand(new[] { "GiveR", "GR" })]
        public async Task<string> GiveRole(ulong guildId, ulong userId, ulong roleId)
        {
            var guild = Context.Client.GetGuild(guildId);
            var user = guild.GetUser(userId);
            var role = guild.GetRole(roleId);
            await user.AddRoleAsync(role);
            return "Done";
        }
        [DevCommand(new[] { "UB" })]
        public async Task<string> Unban(ulong guildId, ulong userId)
        {
            var guild = Context.Client.GetGuild(guildId);
            await guild.RemoveBanAsync(userId);
            return "Done";
        }
    }

    public class DevCommandAttribute : Attribute
    {
        public string[] Aliases { get; }
        public DevCommandAttribute()
        {
            Aliases = new string[0];
        }
        public DevCommandAttribute(string[] aliases)
        {
            Aliases = aliases;
        }
    }
}
