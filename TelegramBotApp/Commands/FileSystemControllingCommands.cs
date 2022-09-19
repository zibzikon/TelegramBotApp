using System.Diagnostics;
using TelegramBotApp.Commands.Arguments;

namespace TelegramBotApp.Commands;

public class OpenFileCommand : ICommand
{
    public async void ExecuteAsync(ICommandArguments commandArguments, CancellationToken cancellationToken = default)
    {
        var telegramBotClient = commandArguments.TelegramBotClient;
        
        await telegramBotClient.SendTextMessageAsync("Send file what you want to open. " +
                                                      "If you want to send photo, audio, or video make it as file");

        var nextUserMessage = await telegramBotClient.GetNextUserMessageAsync();

        if (nextUserMessage is {Document: { } document} == false)
        {
            await telegramBotClient.SendTextMessageAsync("Unknown message type. please send it as file");
            return;
        }
        
        await telegramBotClient.SendTextMessageAsync("The file has been accepted. Starting processing");
        
        var fileId = document.FileId;
        var fileInfo = await telegramBotClient.GetFileAsync(fileId);
        
        if(fileInfo.FilePath is {} filePath == false || document.FileName is{} fileName == false) return;
        
        await using var fileStream = File.OpenWrite(GetFileDownloadPath(fileName));
        
        await telegramBotClient.DownloadFileAsync(filePath: filePath, destination: fileStream, cancellationToken: cancellationToken);
        fileStream.Close();


        new HideAllWindowsCommand().ExecuteAsync(null, cancellationToken);
        
        var pathToFile = GetFileDownloadPath(fileName);
        var processStartInfo = new ProcessStartInfo { FileName = pathToFile, UseShellExecute = true };
        Process.Start(processStartInfo);
        await telegramBotClient.SendTextMessageAsync("File was opened");
    }
        
    private  string GetFileDownloadPath(string fileName)
    {
        return @$"{ApplicationSettings.DownloadDirectory}{fileName}";
    }
}

public class OpenFileByPathCommand: ICommand
{
    public async void ExecuteAsync(ICommandArguments commandArguments, CancellationToken cancellationToken)
    {
        var telegramBotClient = commandArguments.TelegramBotClient;

        var argument = await commandArguments.GetNextArgumentAsync(
            "Now write path to file on your computer. Correct path to file pattern is [DISKNAME:/DIRECTORYNAME/../FILENAME.exe]");

        var pathToFile = argument.Message.Text;
        

        if (pathToFile == null)
        {
            await telegramBotClient.SetException("Invalid command arguments. Correct path to file pattern is [DISKNAME:/DIRECTORYNAME/../FILENAME.exe]");
            return;
        }

        if (File.Exists(pathToFile) == false)
        {
            await telegramBotClient.SetException("File not exists");
            return;
        }
 
        var processStartInfo = new ProcessStartInfo { FileName = pathToFile, UseShellExecute = true };
        try
        {
            Process.Start(processStartInfo);
        }
        catch (Exception exception)
        {
            await telegramBotClient.SetException(exception.Message);
            return;
        }
        
        await telegramBotClient.SendTextMessageAsync("File opening process is started");
    }

}