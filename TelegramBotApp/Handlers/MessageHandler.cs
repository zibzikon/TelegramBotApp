using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotApp.Adapters;
using TelegramBotApp.Commands;
using TelegramBotApp.Extensions;

namespace TelegramBotApp.Handlers;

public class MessageHandler : IMessageHandler
{
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

    private readonly TelegramBotClientAdapter _telegramBotClientAdapter;
    public MessageHandler(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClientAdapter = new TelegramBotClientAdapter(telegramBotClient: telegramBotClient,
            messageHandler: this);
    }

    private bool TryGetCommandByTextMessage(string text, out ICommand? command)
    {
        command = null;
        
        switch (text)
        {
            case "/start": command = new StartCommand(); break;
            case "/shutdownpc": command = new ShutdownPcCommand(); break;
            case "/restartpc": command = new RestartPcCommand(); break;
            case "/hidepcwindows": command = new HideAllWindowsCommand(); break;
            case "/openfile": command = new OpenFileCommand(); break;
            
            default: return false;
        }

        return true;
    }
    
    public async Task HandleMessageAsync(Message message)
    {
        if (message.IsNullOrEmpty())
            return;

        if (_messageWaiter.IsMessageWaiting)
        {
            _messageWaiter.SetWaitingMessage(message);
            return;
        }
        
        if(message.Text is{} messageText == false) return;
        
        if (TryGetCommandByTextMessage(messageText, out var command) == false)
        {
            ExecuteCommand(command: new UnknownMessageCommand(), message: message);
            return;
        }
        
        ExecuteCommand(command: command, message: message);

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

    private void ExecuteCommand(ICommand? command, Message message)
    {
        if (command is null)
            throw new NullReferenceException("Command is null, but you trying to access it");
        
        _telegramBotClientAdapter.GoNewUpdateState(message: message);
        command.Execute(_telegramBotClientAdapter);
    }
}