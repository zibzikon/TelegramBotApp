using Telegram.Bot.Types;

namespace TelegramBotApp.Commands.Arguments;

public interface ICommandArgument
{
    public Message Message{get;}
}