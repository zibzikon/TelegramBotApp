using TelegramBotApp.Adapters;
using TelegramBotApp.Commands;

namespace TelegramBotApp.Containers;

public interface ICommandsContainer
{
     public bool TryGetCommandByTextMessage(string text, ITelegramBotClientAdapter telegramBotClientAdapter,
        out ICommand? command);
}