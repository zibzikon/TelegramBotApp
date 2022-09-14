using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotApp.Handlers;

public interface ICommand
{
    public void Execute(ITelegramBotClient telegramBotClient, Message message);
}