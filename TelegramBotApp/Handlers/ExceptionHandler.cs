using Telegram.Bot;

namespace TelegramBotApp.Handlers;

public class ExceptionHandler : IExceptionHandler
{
    private readonly ITelegramBotClient _telegramBotClient;

    public ExceptionHandler(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    public void HandleException(Exception exception)
    {
    }
}