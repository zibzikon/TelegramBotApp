using TelegramBotApp.Adapters;

namespace TelegramBotApp.Commands.Arguments;

public interface ICommandArguments
{
    public ITelegramBotClientAdapter TelegramBotClient { get; }
    
    public Task<ICommandArgument> GetNextArgumentAsync(string? messageToUser = default);

}