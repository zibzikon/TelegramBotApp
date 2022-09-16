using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotApp.Adapters;

namespace TelegramBotApp.Commands;

public interface ICommand
{
    public void Execute(CommandArguments? commandArguments);
}