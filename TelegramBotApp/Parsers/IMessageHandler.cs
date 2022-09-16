using Telegram.Bot.Types;

namespace TelegramBotApp.Parsers;

public interface IMessageHandler
{
    public Task HandleMessageAsync(Message message);
    
    public Task<Message> GetNextUserMessageAsync();
}