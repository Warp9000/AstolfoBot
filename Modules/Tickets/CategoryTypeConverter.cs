using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace AstolfoBot.Modules.Tickets
{
    public class CategoryTypeConverter<T> : TypeConverter<T> where T : SocketCategoryChannel
    {
        public override ApplicationCommandOptionType GetDiscordType() => ApplicationCommandOptionType.String;

        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
        {
            var guild = context.Guild as SocketGuild;
            var categories = guild?.CategoryChannels;
            var categoryNames = categories?.Select(category => category.Name) ?? Enumerable.Empty<string>();
            var categoryName = option.Value.ToString();
            if (categoryNames.Contains(categoryName))
            {
                return Task.FromResult(TypeConverterResult.FromSuccess(categories?.FirstOrDefault(category => category.Name == categoryName) as T));
            }
            return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ParseFailed, "Category does not exist"));
        }
    }
}