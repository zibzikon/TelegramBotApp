using Telegram.Bot.Types;
using TelegramBotApp.Handlers;
using File = Telegram.Bot.Types.File;

namespace TelegramBotApp.Adapters;

public interface ITelegramBotClientAdapter
{    
    public Message Message { get; }
    
    public ICommandHandler CommandHandler { get; }
    
    public Task SendTextMessageAsync(string textMessage);

    public Task SetException(string exceptionText);
    
    public Task<Message> GetNextUserMessageAsync();

    public Task<File> GetFileAsync(string fileId);
    
    public Task DownloadFileAsync(string filePath, Stream destination, CancellationToken cancellationToken = default);
}