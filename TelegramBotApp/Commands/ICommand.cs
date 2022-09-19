using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotApp.Adapters;
using TelegramBotApp.Commands.Arguments;

namespace TelegramBotApp.Commands;

public interface ICommand
{
    public void ExecuteAsync(ICommandArguments commandArguments, CancellationToken cancellationToken = default);
}