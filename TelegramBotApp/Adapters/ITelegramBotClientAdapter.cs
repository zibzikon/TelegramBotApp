using Telegram.Bot.Types;

namespace TelegramBotApp.Handlers;

public interface ITelegramBotClientAdapter
{
    public Message Message { get; }
    
    public Task SendTextMessageAsync(string message);
}