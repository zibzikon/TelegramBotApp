using Telegram.Bot.Types;

namespace TelegramBotApp.Extensions;

public static class MessageExtensions
{
    public static bool IsNullOrEmpty(this Message? message)
    {
        if (message is null)
            return true;
        
        if (message.Text is not null) return false;
        if (message.Document is not null) return false;
        if (message.Photo is not null) return false;
        
        return true;
    }
}