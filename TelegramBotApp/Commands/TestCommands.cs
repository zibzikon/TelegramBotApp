using System.Diagnostics;
using System.Runtime.InteropServices;
using TelegramBotApp.Adapters;
using File = System.IO.File;

namespace TelegramBotApp.Commands;

public class StartCommand : ICommand
{
    public void Execute(ITelegramBotClientAdapter telegramBotClient)
    {
        telegramBotClient.SendTextMessageAsync("Hello my friend");
    }
}

public class UnknownMessageCommand : ICommand
{
    public void Execute(ITelegramBotClientAdapter telegramBotClient)
    {
        new EchoCommand().Execute(telegramBotClient);
    }
}

public class EchoCommand : ICommand
{
    public void Execute(ITelegramBotClientAdapter telegramBotClient)
    {
        var message = telegramBotClient.Message;
        telegramBotClient.SendTextMessageAsync(
            !string.IsNullOrEmpty(message.Text) ? message.Text : "You send no text");
    }
}

public class ShutdownPcCommand : ICommand
{
    public void Execute(ITelegramBotClientAdapter telegramBotClient)
    {
        telegramBotClient.SendTextMessageAsync("Your pc shutdown shortly");
        
        Process.Start("shtdown", "/s, /t 0");
    }
}

public class RestartPcCommand : ICommand
{
    public void Execute(ITelegramBotClientAdapter telegramBotClient)
    {
        telegramBotClient.SendTextMessageAsync("Your pc restarts shortly");
        
        Process.Start("shtdown", "/r, /t 0");
    }
}

public class OpenFileCommand : ICommand
{
    public async void Execute(ITelegramBotClientAdapter telegramBotClient)
    {
        await telegramBotClient.SendTextMessageAsync("Please send file what you want to open. If you want to send photo, audio, or video make it as file");

        var nextUserMessage = await telegramBotClient.GetNextUserMessageAsync();

        if (nextUserMessage is {Document: { } document} == false)
        {
            await telegramBotClient.SendTextMessageAsync("Unknown message type. please send it as file");
            return;
        }
        
        var fileId = document.FileId;
        var fileInfo = await telegramBotClient.GetFileAsync(fileId);
        
        if(fileInfo.FilePath is {} filePath == false || document.FileName is{} fileName == false) return;
            
        await using var fileStream = File.OpenWrite(GetFileDownloadPath(fileName));
            
        await telegramBotClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
        fileStream.Close();

        new HideAllWindowsCommand().Execute(telegramBotClient);
            
        Process.Start(GetFileDownloadPath(fileName));
        await telegramBotClient.SendTextMessageAsync("File was opened");
    }
        
    private  string GetFileDownloadPath(string fileName)
    {
        return @$"{ApplicationSettings.DownloadDirectory}{fileName}";
    }
}

public class HideAllWindowsCommand : ICommand
{
    const int WmCommand = 0x111;
    const int MinAll = 419;
    
    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string? lpWindowName);
    
    [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
    static extern IntPtr SendMessage(IntPtr hWnd, Int32 msg, IntPtr wParam, IntPtr lParam);
    
    public async void Execute(ITelegramBotClientAdapter telegramBotClient)
    {
        IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
        SendMessage(lHwnd, WmCommand, (IntPtr)MinAll, IntPtr.Zero);
        
        await telegramBotClient.SendTextMessageAsync("All windows on your pc was hided");
    }
}