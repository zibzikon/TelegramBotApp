using System.Text.RegularExpressions;
using TelegramBotApp.Handlers;

namespace TelegramBotApp.Parsers;

public class TextMessageParser : ITextMessageParser
{
    public bool TryGetCommandInStringMessage(string textMessage, out string commandTextResult)
    {
        textMessage = textMessage.Trim();
        
        var regex = new Regex(@"(?<=^/)[a-z]+");
        commandTextResult = regex.Match(textMessage).Value;
        
        return regex.IsMatch(textMessage);
    }

    public bool TryGetCommandArgumentsInStringMessage(string textMessage, out IEnumerable<CommandArgument> commandArgumentsResult )
    {
        textMessage = textMessage.Trim();
        
        var regex = new Regex(@"(?<=\s/)[a-z]+ ?[/a-zA-Z0-9\.\:]+");
        var matches = regex.Matches(textMessage).ToList();
        var argumentsList = new List<CommandArgument>();
        commandArgumentsResult = argumentsList;

        if (matches.Any() == false) return false;
        
        
        foreach (var match in matches)
        {
            var commandArgument = match.Value.Split(" ");
            var argument = commandArgument[0];
            
            if (commandArgument.Length < 1 || commandArgument.Length > 2)
                throw new InvalidProgramException();

            var argumentValue = string.Empty;

            if (commandArgument.Length == 2)
                argumentValue = commandArgument[1];
            

            argumentsList.Add(new CommandArgument(argument, argumentValue));
        }

        return true;
    }
}