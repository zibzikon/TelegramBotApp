namespace TelegramBotApp.Handlers;

public interface ITextMessageParser
{
    public bool TryGetCommandInStringMessage(string textMessage, out string commandTextResult);
    public bool TryGetCommandArgumentsInStringMessage(string textMessage, out IEnumerable<CommandArgument> commandArgumentsResult);
}