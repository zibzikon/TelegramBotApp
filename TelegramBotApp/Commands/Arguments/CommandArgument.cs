using Telegram.Bot.Types;

namespace TelegramBotApp.Commands.Arguments;

public class CommandArgument : ICommandArgument
{
    public CommandArgument(Message message)
    {
        Message = message;
    }
    
    public Message Message { get; }
}