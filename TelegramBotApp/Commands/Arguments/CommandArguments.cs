using TelegramBotApp.Adapters;

namespace TelegramBotApp.Commands.Arguments;

public class CommandArguments : ICommandArguments
{
     public ITelegramBotClientAdapter TelegramBotClient { get; }

     private readonly Queue<ICommandArgument> _commandArgumentsQueue = new();
          
     public CommandArguments(ITelegramBotClientAdapter telegramBotClient, IEnumerable<ICommandArgument>? commandArguments = default)
     {
          TelegramBotClient = telegramBotClient;

          if (commandArguments is null) return;
          foreach (var commandArgument in commandArguments)
               _commandArgumentsQueue.Enqueue(commandArgument);
     }

     public async Task<ICommandArgument> GetNextArgumentAsync(string? messageToUser = default)
     {
          if (_commandArgumentsQueue.TryDequeue(out ICommandArgument? commandArgument))
          {
               return commandArgument;
          }
          
          if (messageToUser != null) await TelegramBotClient.SendTextMessageAsync(messageToUser);
          var userMessage = await TelegramBotClient.GetNextUserMessageAsync();
          return new CommandArgument(userMessage);
     }
}