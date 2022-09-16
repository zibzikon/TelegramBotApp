using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotApp.Handlers;
using TelegramBotApp.Parsers;

namespace TelegramBotApp;

public class TelegramBotApplication : IApplication
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IMessageHandler _messageHandler;
    private readonly IExceptionHandler _exceptionHandler;

    public TelegramBotApplication(ITelegramBotClient telegramBotClient, IMessageHandler messageHandler, IExceptionHandler exceptionHandler)
    {
        _telegramBotClient = telegramBotClient;
        _messageHandler = messageHandler;
        _exceptionHandler = exceptionHandler;
    }

    public void Run()
    {
        _telegramBotClient.StartReceiving(updateHandler: UpdateHandler, 
            pollingErrorHandler: PollingErrorHandler, cancellationToken: _cancellationTokenSource.Token);
        
    }

    public void Close()
    {
        _cancellationTokenSource.Cancel();
    }
    
    private async Task UpdateHandler(ITelegramBotClient telegramBotClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        
        if (message is null)
            return;
        
        await _messageHandler.HandleMessageAsync(message);
    }
    
    private Task PollingErrorHandler(ITelegramBotClient telegramBotClient, Exception exception, CancellationToken cancellationToken)
    {
        _exceptionHandler.HandleException(exception);
        return Task.CompletedTask;
    }
}