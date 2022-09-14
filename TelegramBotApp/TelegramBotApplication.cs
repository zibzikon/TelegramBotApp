using Telegram.Bot;
using Telegram.Bot.Types;

public class TelegramBotApplication
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IMessageHandler _messageHandler;

    public TelegramBotApplication(ITelegramBotClient telegramBotClient, IMessageHandler messageHandler)
    {
        _telegramBotClient = telegramBotClient;
        _messageHandler = messageHandler;
    }

    public void Run()
    {
        _telegramBotClient.StartReceiving(updateHandler: UpdateHandler, 
            pollingErrorHandler: PollingErrorHandler, cancellationToken: _cancellationTokenSource.Token);
        
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
    }
    
    private async Task UpdateHandler(ITelegramBotClient telegramBotClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        await _messageHandler.HandleMessageAsync(message);
    }
    
    private Task PollingErrorHandler(ITelegramBotClient telegramBotClient, Exception exception, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}