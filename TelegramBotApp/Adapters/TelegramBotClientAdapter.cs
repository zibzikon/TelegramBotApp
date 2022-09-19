using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotApp.Handlers;
using File = Telegram.Bot.Types.File;

namespace TelegramBotApp.Adapters;

public class TelegramBotClientAdapter : ITelegramBotClientAdapter
{
    public Message Message { get; }

    public ICommandHandler CommandHandler { get; }
    
    private readonly ITelegramBotClient _telegramBotClient;

    private readonly IMessageHandler _messageHandler;
    
    public TelegramBotClientAdapter( ITelegramBotClient telegramBotClient, IMessageHandler messageHandler,
        ICommandHandler commandHandler, Message message)
    {
        Message = message;
        _messageHandler = messageHandler;
        CommandHandler = commandHandler;
        _telegramBotClient = telegramBotClient;
    }
    
    public async Task SendTextMessageAsync(string textMessage)
    {
        await _telegramBotClient.SendTextMessageAsync(chatId: Message.Chat.Id, textMessage);
    }

    public async Task SetException(string exceptionText)
    {
        await SendTextMessageAsync($"Exception:{exceptionText}");
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