using System.Text.RegularExpressions;
using Discord;
using Discord.Interactions;

namespace AstolfoBot.Modules.Embeds
{
    [RequireUserPermission(GuildPermission.ManageMessages)]
    public class EmbedsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("fastembed", "Sends an embed")]
        public async Task FastEmbedCommand(
            [Summary("title", "The title of the embed")] string? title = null,
            [Summary("description", "The description of the embed")] string? description = null,
            [Summary("colorhex", "The color of the embed")] string? colorhex = null,
            [Summary("footer", "The footer of the embed")] string? footer = null,
            [Summary("footericonurl", "The footer icon url of the embed")] string? footericonurl = null,
            [Summary("imageurl", "The image url of the embed")] string? imageurl = null,
            [Summary("thumbnailurl", "The thumbnail url of the embed")] string? thumbnailurl = null,
            [Summary("author", "The author of the embed")] string? author = null,
            [Summary("authoriconurl", "The author icon url of the embed")] string? authoriconurl = null,
            [Summary("authorurl", "The author url of the embed")] string? authorurl = null,
            [Summary("url", "The url of the embed")] string? url = null,

            [Summary("channel", "The channel to send the embed to")] ITextChannel? channel = null
        )
        {
            try
            {
                int totalLength = 0;
                var embed = new EmbedBuilder();
                if (title != null)
                {
                    if (title.Length > EmbedBuilder.MaxTitleLength)
                    {
                        await RespondAsync("Title is too long. Please use a title that is less than 256 characters.");
                        return;
                    }
                    embed.WithTitle(title);
                    totalLength += title.Length;
                }
                if (description != null)
                {
                    if (description.Length > EmbedBuilder.MaxDescriptionLength)
                    {
                        await RespondAsync("Description is too long. Please use a description that is less than 4096 characters.");
                        return;
                    }
                    embed.WithDescription(description);
                    totalLength += description.Length;
                }
                if (colorhex != null)
                {
                    // regex that only matches a 6 digit hex color code
                    if (Regex.IsMatch(colorhex, @"^#([A-Fa-f0-9]{6})$"))
                    {
                        colorhex = colorhex.TrimStart('#');
                        uint colorint = uint.Parse(colorhex, System.Globalization.NumberStyles.HexNumber);
                        embed.WithColor(colorint);
                        totalLength += colorhex.Length;
                    }
                    else
                    {
                        await RespondAsync("Invalid color hex. Please use a valid hex color code. Example: #FF0000");
                        return;
                    }
                }

                embed.WithFooter(
                    footer +
                $"{(string.IsNullOrEmpty(footer) ? "" : " | ")}Custom embed sent by {Context.User.Username}#{Context.User.Discriminator}",
                footericonurl ?? "");
                totalLength += (footer == null ? 0 : footer!.Length) + (footericonurl == null ? 0 : footericonurl!.Length);

                if (imageurl != null)
                {
                    embed.WithImageUrl(imageurl);
                    totalLength += imageurl.Length;
                }
                if (thumbnailurl != null)
                {
                    embed.WithThumbnailUrl(thumbnailurl);
                    totalLength += thumbnailurl.Length;
                }
                if (author != null)
                {
                    embed.WithAuthor(author, authoriconurl, authorurl);
                    totalLength += author.Length + authoriconurl == null ? 0 : authoriconurl!.Length + authorurl == null ? 0 : authorurl!.Length;
                }
                if (url != null)
                {
                    embed.WithUrl(url);
                    totalLength += url.Length;
                }
                await (channel ?? Context.Channel as ITextChannel)!.SendMessageAsync(embed: embed.Build());
                await RespondAsync($"Embed sent to {(channel ?? Context.Channel as ITextChannel)!.Mention}");
            }
            catch (Exception e)
            {
                Logger.Error("An error occurred while sending an embed", this, e);
                await RespondAsync($"An error occurred");
            }
        }
        [Group("embed", "Sends an embed")]
        public class EasyEmbedModule : InteractionModuleBase<SocketInteractionContext>
        {
            static Dictionary<ulong, EasyEmbed> EasyEmbeds { get; set; } = new Dictionary<ulong, EasyEmbed>();
            [SlashCommand("start", "Starts an embed")]
            public async Task Start()
            {
                try
                {
                    if (EasyEmbeds.ContainsKey(Context.User.Id))
                    {
                        await RespondAsync("You already have an embed started");
                        return;
                    }
                    var embed = new EmbedBuilder()
                        .WithTitle("Title")
                        .WithDescription("Description")
                        .WithColor(Discord.Color.Red)
                        .WithFooter($"Footer | Custom embed by {Context.User.Username}#{Context.User.Discriminator}")
                        .WithAuthor("Author");
                    var message = await ReplyAsync(embed: embed.Build());
                    EasyEmbeds.Add(Context.User.Id, new EasyEmbed(embed, message));
                    await RespondAsync("Embed started");
                }
                catch (Exception e)
                {
                    Logger.Error("An error occurred while starting an embed", this, e);
                    await RespondAsync($"An error occurred");
                }
            }
            [SlashCommand("stop", "Stops an embed")]
            public async Task Stop()
            {
                try
                {
                    if (EasyEmbeds.ContainsKey(Context.User.Id))
                    {
                        EasyEmbeds.Remove(Context.User.Id);
                        await RespondAsync("Embed stopped");
                    }
                    else
                    {
                        await RespondAsync("You don't have an embed started");
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("An error occurred while stopping an embed", this, e);
                    await RespondAsync($"An error occurred");
                }
            }
            [SlashCommand("select", "Selects an alreadt sent embed")]
            public async Task Select([Summary("message", "The message id of the embed")] ulong? messageid = null)
            {
                await RespondAsync("Not implemented yet due to security concerns");
            }

            [SlashCommand("title", "Sets the title of the embed")]
            public async Task Title([Summary("title", "The title of the embed")] string? title = null)
            {
                if (EasyEmbeds.TryGetValue(Context.User.Id, out var easyEmbed))
                {
                    if (title != null)
                    {
                        if (title.Length > EmbedBuilder.MaxTitleLength)
                        {
                            await RespondAsync("Title is too long. Please use a title that is less than 256 characters.");
                            return;
                        }
                        easyEmbed.Embed.WithTitle(title);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Title set");
                    }
                    else
                    {
                        easyEmbed.Embed.WithTitle(null);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Title removed");
                    }
                }
                else
                {
                    await RespondAsync("No embed started");
                }
            }
            [SlashCommand("description", "Sets the description of the embed")]
            public async Task Description([Summary("description", "The description of the embed")] string? description = null)
            {
                if (EasyEmbeds.TryGetValue(Context.User.Id, out var easyEmbed))
                {
                    if (description != null)
                    {
                        if (description.Length > EmbedBuilder.MaxDescriptionLength)
                        {
                            await RespondAsync("Description is too long. Please use a description that is less than 4096 characters.");
                            return;
                        }
                        easyEmbed.Embed.WithDescription(description);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Description set");
                    }
                    else
                    {
                        easyEmbed.Embed.WithDescription(null);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Description removed");
                    }
                }
                else
                {
                    await RespondAsync("No embed started");
                }
            }
            [SlashCommand("color", "Sets the color of the embed")]
            public async Task Color([Summary("color", "The color of the embed. Must be in #000000 format")] string? colorhex = null)
            {
                if (EasyEmbeds.TryGetValue(Context.User.Id, out var easyEmbed))
                {
                    if (colorhex != null)
                    {
                        if (Regex.IsMatch(colorhex, @"^#([A-Fa-f0-9]{6})$"))
                        {
                            colorhex = colorhex.TrimStart('#');
                            uint colorint = uint.Parse(colorhex, System.Globalization.NumberStyles.HexNumber);
                            easyEmbed.Embed.WithColor(new Color(colorint));
                            await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                            await RespondAsync("Color set");
                        }
                        else
                        {
                            await RespondAsync("Invalid color hex. Please use a valid hex color code. Example: #FF0000");
                            return;
                        }
                    }
                    else
                    {
                        easyEmbed.Embed.WithColor(default);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Color removed");
                    }
                }
                else
                {
                    await RespondAsync("No embed started");
                }
            }
            [SlashCommand("url", "Sets the url of the embed")]
            public async Task Url([Summary("url", "The url of the embed")] string? url = null)
            {
                if (EasyEmbeds.TryGetValue(Context.User.Id, out var easyEmbed))
                {
                    if (url != null)
                    {
                        easyEmbed.Embed.WithUrl(url);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Url set");
                    }
                    else
                    {
                        easyEmbed.Embed.WithUrl(null);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Url removed");
                    }
                }
                else
                {
                    await RespondAsync("No embed started");
                }
            }
            [SlashCommand("image", "Sets the image of the embed")]
            public async Task Image([Summary("image", "The image url of the embed")] string? image = null)
            {
                if (EasyEmbeds.TryGetValue(Context.User.Id, out var easyEmbed))
                {
                    if (image != null)
                    {
                        easyEmbed.Embed.WithImageUrl(image);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Image set");
                    }
                    else
                    {
                        easyEmbed.Embed.WithImageUrl(null);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Image removed");
                    }
                }
                else
                {
                    await RespondAsync("No embed started");
                }
            }
            [SlashCommand("thumbnail", "Sets the thumbnail url of the embed")]
            public async Task Thumbnail([Summary("thumbnail", "The thumbnail url of the embed")] string? thumbnail = null)
            {
                if (EasyEmbeds.TryGetValue(Context.User.Id, out var easyEmbed))
                {
                    if (thumbnail != null)
                    {
                        easyEmbed.Embed.WithThumbnailUrl(thumbnail);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Thumbnail set");
                    }
                    else
                    {
                        easyEmbed.Embed.WithThumbnailUrl(null);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Thumbnail removed");
                    }
                }
                else
                {
                    await RespondAsync("No embed started");
                }
            }
            [SlashCommand("author", "Sets the author of the embed")]
            public async Task Author([Summary("author", "The author of the embed")] string? author = null)
            {
                if (EasyEmbeds.TryGetValue(Context.User.Id, out var easyEmbed))
                {
                    if (author != null)
                    {
                        easyEmbed.Embed.WithAuthor(author, easyEmbed.Embed.Author.IconUrl, easyEmbed.Embed.Author.Url);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Author set");
                    }
                    else
                    {
                        easyEmbed.Embed.WithAuthor("", easyEmbed.Embed.Author.IconUrl, easyEmbed.Embed.Author.Url);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Author removed");
                    }
                }
                else
                {
                    await RespondAsync("No embed started");
                }
            }
            [SlashCommand("authoricon", "Sets the author icon url of the embed")]
            public async Task AuthorIcon([Summary("authoricon", "The author icon url of the embed")] string? authoricon = null)
            {
                if (EasyEmbeds.TryGetValue(Context.User.Id, out var easyEmbed))
                {
                    if (authoricon != null)
                    {
                        easyEmbed.Embed.WithAuthor(easyEmbed.Embed.Author.Name, authoricon, easyEmbed.Embed.Author.Url);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Author icon set");
                    }
                    else
                    {
                        easyEmbed.Embed.WithAuthor(easyEmbed.Embed.Author.Name, "", easyEmbed.Embed.Author.Url);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Author icon removed");
                    }
                }
                else
                {
                    await RespondAsync("No embed started");
                }
            }
            [SlashCommand("authorurl", "Sets the author url of the embed")]
            public async Task AuthorUrl([Summary("authorurl", "The author url of the embed")] string? authorurl = null)
            {
                if (EasyEmbeds.TryGetValue(Context.User.Id, out var easyEmbed))
                {
                    if (authorurl != null)
                    {
                        easyEmbed.Embed.WithAuthor(easyEmbed.Embed.Author.Name, easyEmbed.Embed.Author.IconUrl, authorurl);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Author url set");
                    }
                    else
                    {
                        easyEmbed.Embed.WithAuthor(easyEmbed.Embed.Author.Name, easyEmbed.Embed.Author.IconUrl, "");
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Author url removed");
                    }
                }
                else
                {
                    await RespondAsync("No embed started");
                }
            }
            [SlashCommand("footer", "Sets the footer of the embed")]
            public async Task Footer([Summary("footer", "The footer of the embed")] string? footer = null)
            {
                if (EasyEmbeds.TryGetValue(Context.User.Id, out var easyEmbed))
                {
                    if (footer != null)
                    {
                        easyEmbed.Embed.WithFooter(footer +
                            $" | Custom embed by {Context.User.Username}#{Context.User.Discriminator}", easyEmbed.Embed.Footer.IconUrl);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Footer set");
                    }
                    else
                    {
                        easyEmbed.Embed.WithFooter(
                            $"Custom embed by {Context.User.Username}#{Context.User.Discriminator}", easyEmbed.Embed.Footer.IconUrl);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Footer removed");
                    }
                }
                else
                {
                    await RespondAsync("No embed started");
                }
            }
            [SlashCommand("footericon", "Sets the footer icon url of the embed")]
            public async Task FooterIcon([Summary("footericon", "The footer icon url of the embed")] string? footericon = null)
            {
                if (EasyEmbeds.TryGetValue(Context.User.Id, out var easyEmbed))
                {
                    if (footericon != null)
                    {
                        easyEmbed.Embed.WithFooter(easyEmbed.Embed.Footer.Text, footericon);
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Footer icon set");
                    }
                    else
                    {
                        easyEmbed.Embed.WithFooter(easyEmbed.Embed.Footer.Text, "");
                        await easyEmbed.Message.ModifyAsync(x => x.Embed = easyEmbed.Embed.Build());
                        await RespondAsync("Footer icon removed");
                    }
                }
                else
                {
                    await RespondAsync("No embed started");
                }
            }

            [SlashCommand("send", "Sends the embed")]
            public async Task Send([Summary("channel", "The channel to send the embed to")] ITextChannel? channel = null)
            {
                if (EasyEmbeds.TryGetValue(Context.User.Id, out var easyEmbed))
                {
                    if (channel == null)
                    {
                        channel = (ITextChannel)Context.Channel;
                    }
                    await channel.SendMessageAsync(embed: easyEmbed.Embed.Build());
                    await RespondAsync("Embed sent");
                    EasyEmbeds.Remove(Context.User.Id);
                }
                else
                {
                    await RespondAsync("No embed started");
                }
            }
        }

        public struct EasyEmbed
        {
            public EasyEmbed(EmbedBuilder embed, IUserMessage message)
            {
                Embed = embed;
                Message = message;
            }
            public IUserMessage Message { get; set; }
            public EmbedBuilder Embed { get; set; }
        }
    }
}