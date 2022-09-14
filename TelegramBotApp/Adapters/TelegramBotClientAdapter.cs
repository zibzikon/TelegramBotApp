using Telegram.Bot;
using Telegram.Bot.Types;
using File = Telegram.Bot.Types.File;

namespace TelegramBotApp.Adapters;

public class TelegramBotClientAdapter : ITelegramBotClientAdapter
{
    private readonly ITelegramBotClient _telegramBotClient;

    private readonly IMessageHandler _messageHandler;
    private Message? _message;

    public Message Message
    {
        get
        {
            if (_message is null)
                throw new InvalidProgramException($"Trying access to message before initializing {GetType()}");
            
            return _message;
        }
    }

    public TelegramBotClientAdapter( ITelegramBotClient telegramBotClient, IMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
        _telegramBotClient = telegramBotClient;
    }

    public void GoNewUpdateState(Message message)
    {
        _message = message;
    }
    
    public async Task SendTextMessageAsync(string textMessage)
    {
        await _telegramBotClient.SendTextMessageAsync(chatId: Message.Chat.Id, textMessage);
    }

    public async Task<Message> GetNextUserMessageAsync()
    {
       return await _messageHandler.GetNextUserMessageAsync();
    }

    public async Task<File> GetFileAsync(string fileId)
    {
       return await _telegramBotClient.GetFileAsync(fileId);
    }

    public async Task DownloadFileAsync(string filePath, Stream destination, CancellationToken cancellationToken)
    { 
        await _telegramBotClient.DownloadFileAsync(filePath, destination, cancellationToken);
    }
}