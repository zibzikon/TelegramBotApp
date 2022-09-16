using Telegram.Bot.Types;

namespace TelegramBotApp;

public readonly struct CommandArguments
{
     public IEnumerable<CommandArgument>? Arguments { get; }
     
     public Message Message { get; }

     public CommandArguments(IEnumerable<CommandArgument>? arguments, Message message)
     {
          Arguments = arguments;
          Message = message;
     }

     public bool TryGetArgumentValueByType(string argumentType, out string argumentValue)
     {
          argumentValue = string.Empty;
          
          if (Arguments is null)
               return false;
          
          foreach (var commandArgument in Arguments)
          {
               if (commandArgument.Argument != argumentType) continue;
               
               argumentValue = commandArgument.Value;
               return true;
          }

          return false;
     }
}