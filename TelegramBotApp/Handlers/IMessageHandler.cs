using Telegram.Bot.Types;

public interface IMessageHandler
{
    public Task HandleMessageAsync(Message message);
}