using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotApp.Adapters;
using TelegramBotApp.Commands;
using TelegramBotApp.Containers;
using TelegramBotApp.Extensions;
using TelegramBotApp.Parsers;

namespace TelegramBotApp.Handlers;

public class MessageHandler : IMessageHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    
    private readonly ICommandsContainer _commandsContainer;

    private class MessageWaiter
    {
        public bool IsMessageWaiting { get; private set; }
        private Message? _waitingMessage;

        public bool TryGetWaitingMessage(out Message? message)
        {
            message = _waitingMessage;
        
            return _waitingMessage is not null;
        }
    
        public void StartWaitingMessage()
        {
            Reset();
            IsMessageWaiting = true;
        }
    
        public void SetWaitingMessage(Message message)
        {
            _waitingMessage = message;
        }

        public void Reset()
        {
            _waitingMessage = null;
            IsMessageWaiting = false;
        }
    }
    
    private readonly MessageWaiter _messageWaiter = new();
    
    private readonly ITextMessageParser _textMessageParser;
    
    public MessageHandler(ITelegramBotClient telegramBotClient, ICommandsContainer commandsContainer,
        ITextMessageParser textMessageParser)
    {
        _telegramBotClient = telegramBotClient;
        _commandsContainer = commandsContainer;
        _textMessageParser = textMessageParser;
    }
    
    public async Task HandleMessageAsync(Message message)
    {
        if (message.IsEmpty())
            return;

        if (_messageWaiter.IsMessageWaiting)
        {
            _messageWaiter.SetWaitingMessage(message);
            return;
        }
        
        if(message.Text is{} messageText == false) return;
        
        var commandArguments = new CommandArguments(arguments:null, message);
        
        if (message.Text is { } textMessage&& _textMessageParser.
                TryGetCommandArgumentsInStringMessage(textMessage, out var arguments))
        {
            commandArguments = new CommandArguments(arguments: arguments, message);
        }

        var telegramBotClientAdapter = new TelegramBotClientAdapter(_telegramBotClient, this, message);
        
        if (_textMessageParser.TryGetCommandInStringMessage(messageText, out var commandText) == false || 
            _commandsContainer.TryGetCommandByTextMessage(commandText, telegramBotClientAdapter, out var command) == false)
        {
            ExecuteCommand(command: new UnknownMessageCommand(telegramBotClientAdapter), commandArguments: commandArguments);
            return;
        }
        
        ExecuteCommand(command: command, commandArguments: commandArguments);

        await Task.CompletedTask;
    }

    public async Task<Message> GetNextUserMessageAsync()
    {
        _messageWaiter.StartWaitingMessage();
        
        Message? message;
        while (_messageWaiter.TryGetWaitingMessage(out message) == false)
        {
            await Task.Yield();
        }

        if (message is null)
            throw new InvalidProgramException("Message cannot be null");
        
        _messageWaiter.Reset();
        
        return message;
    }

    private void ExecuteCommand(ICommand? command, CommandArguments commandArguments)
    {
        if (command is null)
            throw new NullReferenceException("Command is null, but you trying to access it");

        command.Execute(commandArguments);
    }
}