using Telegram.Bot.Types;
using TelegramBotApp.Adapters;

namespace TelegramBotApp.Handlers;

public interface IMessageHandler
{
    public Task HandleMessageAsync(Message telegramUserMessage, CancellationToken cancellationToken = default);
    
    public Task<Message> GetNextUserMessageAsync(CancellationToken cancellationToken = default);
}