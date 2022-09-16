using TelegramBotApp.Adapters;

namespace TelegramBotApp.Commands;

public class StartCommand : ICommand
{
    private readonly ITelegramBotClientAdapter _telegramBotClient;

    public StartCommand(ITelegramBotClientAdapter telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    public void Execute(CommandArguments? commandArguments)
    {
        _telegramBotClient.SendTextMessageAsync("Hello my friend");
    }
}

public class UnknownMessageCommand : ICommand
{
    private readonly ITelegramBotClientAdapter _telegramBotClient;

    public UnknownMessageCommand(ITelegramBotClientAdapter telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    public void Execute(CommandArguments? commandArguments)
    {
        new EchoCommand(_telegramBotClient).Execute(null);
    }
}

public class EchoCommand : ICommand
{
    private readonly ITelegramBotClientAdapter _telegramBotClient;

    public EchoCommand(ITelegramBotClientAdapter telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    public void Execute(CommandArguments? commandArguments)
    {
        var message = _telegramBotClient.Message;
        _telegramBotClient.SendTextMessageAsync(
            !string.IsNullOrEmpty(message.Text) ? $"Echo:{message.Text}" : "You send no text");
    }
}