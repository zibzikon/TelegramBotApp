using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotApp.Adapters;
using TelegramBotApp.Commands;
using TelegramBotApp.Commands.Arguments;
using TelegramBotApp.Containers;
using TelegramBotApp.Extensions;

namespace TelegramBotApp.Handlers;

public class MessageHandler : IMessageHandler , ICommandHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    
    private readonly ICommandsContainer _commandsContainer;

    private TaskCompletionSource<Message> _waitingMessageTaskCompletionSource = new();

    private bool _messageWaiting = false;
    
    
    public MessageHandler(ITelegramBotClient telegramBotClient, ICommandsContainer commandsContainer)
    {
        _telegramBotClient = telegramBotClient;
        _commandsContainer = commandsContainer;
    }
    
    public async Task HandleMessageAsync(Message message, CancellationToken cancellationToken = default)
    {
        if (message.IsEmpty())
            return;
        
        if (_messageWaiting)
        {
            _waitingMessageTaskCompletionSource.SetResult(message);
            _waitingMessageTaskCompletionSource = new TaskCompletionSource<Message>();
            return;
        }
        
        if(message.Text is{} messageText == false) return;

        var telegramBotClientAdapter = new TelegramBotClientAdapter(_telegramBotClient, messageHandler: this, commandHandler: this, message);
        var commandArguments = new CommandArguments(telegramBotClientAdapter, null);
        await HandleCommandTextAsync(messageText, commandArguments, cancellationToken);
        
        await Task.CompletedTask;
    }

    public async Task<Message> GetNextUserMessageAsync(CancellationToken cancellationToken = default)
    {
        _messageWaiting = true;
        var message = await _waitingMessageTaskCompletionSource.Task;
        _messageWaiting = false;
        
        if (message is null)
            throw new InvalidProgramException("Message cannot be null");
        
        return message;
    }

    private static void ExecuteCommand(ICommand? command, CommandArguments commandArguments, CancellationToken cancellationToken)
    {
        if (command is null)
            throw new NullReferenceException("Command is null, but you trying to access it");

        command.ExecuteAsync(commandArguments, cancellationToken);
    }

    public async Task HandleCommandTextAsync(string commandText,
        CommandArguments commandArguments, CancellationToken cancellationToken = default)
    {
        _ = _commandsContainer.TryGetCommandByTextMessage(commandText, out var command) == false;
        
        await HandleCommandAsync(command, commandArguments, cancellationToken);

        await Task.CompletedTask;
    }

    public Task HandleCommandAsync(ICommand command, CommandArguments commandArguments,
        CancellationToken cancellationToken = default)
    {
        ExecuteCommand(command, commandArguments, cancellationToken);
        
        return Task.CompletedTask;
    }
}