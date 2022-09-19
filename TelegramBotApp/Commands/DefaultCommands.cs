using TelegramBotApp.Adapters;
using TelegramBotApp.Commands.Arguments;
using TelegramBotApp.Containers;

namespace TelegramBotApp.Commands;

public class StartCommand : ICommand
{
    public void ExecuteAsync(ICommandArguments commandArguments, CancellationToken cancellationToken = default)
    {
        var telegramBotClient = commandArguments.TelegramBotClient;
        
        telegramBotClient.SendTextMessageAsync("Hello my friend");
    }
}

public class UnknownMessageCommand : ICommand
{
    public void ExecuteAsync(ICommandArguments commandArguments, CancellationToken cancellationToken = default)
    {
        new EchoCommand().ExecuteAsync(commandArguments, cancellationToken);
    }
}

public class EchoCommand : ICommand
{
    public void ExecuteAsync(ICommandArguments commandArguments, CancellationToken cancellationToken = default)
    {
        var telegramBotClient = commandArguments.TelegramBotClient;
        var message = telegramBotClient.Message;
        telegramBotClient.SendTextMessageAsync(
            !string.IsNullOrEmpty(message.Text) ? $"Echo:{message.Text}" : "You send no text");
    }
}