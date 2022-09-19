using Telegram.Bot.Requests;
using TelegramBotApp.Adapters;
using TelegramBotApp.Commands.Arguments;
using TelegramBotApp.Containers;

namespace TelegramBotApp.Commands;

public class ExecuteAliasCommandCommand : ICommand
{
    
    private readonly ICommandAliasesContainer _commandAliasesContainer;
    private readonly ICommandsContainer _commandsContainer;

    public ExecuteAliasCommandCommand(ICommandAliasesContainer commandAliasesContainer, ICommandsContainer commandsContainer)
    {
        _commandAliasesContainer = commandAliasesContainer;
        _commandsContainer = commandsContainer;
    }
    
    public async void ExecuteAsync(ICommandArguments commandArguments, CancellationToken cancellationToken = default)
    {
        var telegramBotClient = commandArguments.TelegramBotClient;
        
        var userMessageArgument = await commandArguments.GetNextArgumentAsync("Write alias name");
        
        if (userMessageArgument.Message.Text is {} alias == false)
        {
            await telegramBotClient.SetException("Invalid argument");
            return;
        }

        var commandBlank = await _commandAliasesContainer.GetAliasCommandAsync(alias, cancellationToken);

        if (commandBlank is null)
        {
            await telegramBotClient.SetException("Unknown alias");
            return;
        }

        var commandExist = _commandsContainer.TryGetCommandByTextMessage(commandBlank.CommandText, out var command);
      
        if (commandExist == false)
        {
            await telegramBotClient.SetException("Invalid program exception");
        }
        
        await telegramBotClient.CommandHandler.HandleCommandAsync(command,
            new CommandArguments(telegramBotClient, commandBlank.CommandArguments), cancellationToken);
    }
}

public class CreateAliasCommand : ICommand
{
    private readonly ICommandAliasesContainer _commandAliasesContainer;
    
    public CreateAliasCommand(
        ICommandAliasesContainer commandAliasesContainer)
    {
        _commandAliasesContainer = commandAliasesContainer;
    }
    public async void ExecuteAsync(ICommandArguments commandArguments, CancellationToken cancellationToken = default)
    {
        var telegramBotClient = commandArguments.TelegramBotClient;
        
        var argument = await commandArguments.GetNextArgumentAsync("Write alias name");
        var argumentMessage = argument.Message;
        if (argumentMessage.Text is {} alias == false)
        {
            await SendInvalidArgumentException(telegramBotClient);
            return;
        }

        if (await _commandAliasesContainer.GetAliasCommandAsync(alias, cancellationToken) is not null)
            await telegramBotClient.SetException("Alias with the same name is exists");
        
        
        argument = await commandArguments.GetNextArgumentAsync("Write a command");

        argumentMessage = argument.Message;
        if (argumentMessage.Text is { } command == false)
        {
            await SendInvalidArgumentException(telegramBotClient);
            return;
        }
        argument = await commandArguments.GetNextArgumentAsync("Write a command arguments, To stop add arguments to argument list paste [/break]");
        
        argumentMessage = argument.Message;

        var argumentsList = new List<ICommandArgument>();
        
        while (true)
        {
            if (argumentMessage.Text == "/break") break;
            
            if (argumentMessage.Text is null)
            {
                await SendInvalidArgumentException(telegramBotClient);
                return;
            }

            var commandArgument = new CommandArgument(argumentMessage);
            
            argumentsList.Add(commandArgument);
            
            argument = await commandArguments.GetNextArgumentAsync("Write a next command argument");
            argumentMessage = argument.Message;
        }
        
        var commandBlank = new CommandBlank(command, argumentsList);
        var aliasCommandAdded = await _commandAliasesContainer.TryAddAliasCommandAsync(alias, commandBlank, cancellationToken);

        if (aliasCommandAdded == false)
        {
           await telegramBotClient.SetException("Invalid program exception");
           return;
        }
        
        await telegramBotClient.SendTextMessageAsync("New alias was created");
    }

    private async Task SendInvalidArgumentException(ITelegramBotClientAdapter telegramBotClient)
    { 
        await telegramBotClient.SetException("Invalid argument");
    }
}

public class DeleteAliasCommand : ICommand
{
    private readonly ICommandAliasesContainer _commandAliasesContainer;

    public DeleteAliasCommand(ICommandAliasesContainer commandAliasesContainer)
    {
        _commandAliasesContainer = commandAliasesContainer;
    }
    public async void ExecuteAsync(ICommandArguments commandArguments, CancellationToken cancellationToken = default)
    {
        var telegramBotClient = commandArguments.TelegramBotClient;
        
        await telegramBotClient.SendTextMessageAsync("Write alias name");
        var userMessage = await telegramBotClient.GetNextUserMessageAsync();
        if (userMessage.Text is {} alias == false)
        {
            await telegramBotClient.SetException("Invalid argument exception");
            return;
        }

        if (await _commandAliasesContainer.TryRemoveAliasCommandAsync(alias, cancellationToken) == false)
        {
            await telegramBotClient.SendTextMessageAsync("Unknown alias name");
            return;
        }

        await telegramBotClient.SendTextMessageAsync("Alias is removed");
    }
}