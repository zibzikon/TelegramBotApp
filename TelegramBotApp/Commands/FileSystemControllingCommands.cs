using System.Diagnostics;
using TelegramBotApp.Adapters;

namespace TelegramBotApp.Commands;

public class OpenFileCommand : ICommand
{
    private readonly ITelegramBotClientAdapter _telegramBotClient;

    public OpenFileCommand(ITelegramBotClientAdapter telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    public async void Execute(CommandArguments? commandArguments)
    {
        if (commandArguments is not null)
        {
            var arguments = commandArguments.GetValueOrDefault().Arguments;
            if (arguments is not null && arguments.Any())
            {
                await _telegramBotClient.SendTextMessageAsync("Invalid arguments");
                return;
            }
        }

        await _telegramBotClient.SendTextMessageAsync("Send file what you want to open. " +
                                                      "If you want to send photo, audio, or video make it as file");

        var nextUserMessage = await _telegramBotClient.GetNextUserMessageAsync();

        if (nextUserMessage is {Document: { } document} == false)
        {
            await _telegramBotClient.SendTextMessageAsync("Unknown message type. please send it as file");
            return;
        }
        
        await _telegramBotClient.SendTextMessageAsync("The file has been accepted. Starting processing");
        
        var fileId = document.FileId;
        var fileInfo = await _telegramBotClient.GetFileAsync(fileId);
        
        if(fileInfo.FilePath is {} filePath == false || document.FileName is{} fileName == false) return;
        
        await using var fileStream = File.OpenWrite(GetFileDownloadPath(fileName));
        
        await _telegramBotClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
        fileStream.Close();


        new HideAllWindowsCommand(_telegramBotClient).Execute(null);
        
        var pathToFile = GetFileDownloadPath(fileName);
        var processStartInfo = new ProcessStartInfo { FileName = pathToFile, UseShellExecute = true };
        Process.Start(processStartInfo);
        await _telegramBotClient.SendTextMessageAsync("File was opened");
    }
        
    private  string GetFileDownloadPath(string fileName)
    {
        return @$"{ApplicationSettings.DownloadDirectory}{fileName}";
    }
}

public class OpenFileByPathCommand: ICommand
{
    private readonly ITelegramBotClientAdapter _telegramBotClient;

    public OpenFileByPathCommand(ITelegramBotClientAdapter telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    
    public async void Execute(CommandArguments? commandArguments)
    {
        if (commandArguments is null || commandArguments.GetValueOrDefault().
                TryGetArgumentValueByType("path", out var argumentValue) == false)
        {
            await _telegramBotClient.SendTextMessageAsync(
                "Invalid command arguments. Correct command pattern is [ /openpath /path pathtofile]. Correct path to file pattern is [DISKNAME:/DIRECTORYNAME/../FILENAME.exe]");
            return;
        }

        if (File.Exists(argumentValue) == false)
        {
            await _telegramBotClient.SendTextMessageAsync("File not exists");
            return;
        }
 
        var processStartInfo = new ProcessStartInfo { FileName = argumentValue, UseShellExecute = true };
        
        Process.Start(processStartInfo);
        await _telegramBotClient.SendTextMessageAsync("File opening process is started");
    }
}