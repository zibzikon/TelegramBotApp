using Telegram.Bot.Types;
using TelegramBotApp.Adapters;
using TelegramBotApp.Commands;
using TelegramBotApp.Commands.Arguments;

namespace TelegramBotApp.Handlers;

public interface ICommandHandler
{
    public Task HandleCommandTextAsync(string commandText, 
        CommandArguments commandArguments, CancellationToken cancellationToken = default);

    public Task HandleCommandAsync(ICommand command, CommandArguments commandArguments,
        CancellationToken cancellationToken = default);
}