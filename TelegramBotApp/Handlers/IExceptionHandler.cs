namespace TelegramBotApp.Handlers;

public interface IExceptionHandler
{
    public void HandleException(Exception exception);
}