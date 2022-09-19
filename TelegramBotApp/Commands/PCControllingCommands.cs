using System.Diagnostics;
using System.Runtime.InteropServices;
using TelegramBotApp.Adapters;
using TelegramBotApp.Commands.Arguments;

namespace TelegramBotApp.Commands;
public class ShutdownPcCommand : ICommand
{
    public void ExecuteAsync(ICommandArguments commandArguments, CancellationToken cancellationToken = default)
    {
        var telegramBotClient = commandArguments.TelegramBotClient;

        telegramBotClient.SendTextMessageAsync("Your pc shutdown shortly");
        
        Process.Start("shtdown", "/s, /t 0");
    }
}

public class RestartPcCommand : ICommand
{
    public void ExecuteAsync(ICommandArguments commandArguments, CancellationToken cancellationToken = default)
    {
        var telegramBotClient = commandArguments.TelegramBotClient;

        telegramBotClient.SendTextMessageAsync("Your pc restarts shortly");
        
        Process.Start("shtdown", "/r, /t 0");
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
    
    public async void ExecuteAsync(ICommandArguments commandArguments, CancellationToken cancellationToken = default)
    {
        var telegramBotClient = commandArguments.TelegramBotClient;
        
        IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
        SendMessage(lHwnd, WmCommand, (IntPtr)MinAll, IntPtr.Zero);
        
        await telegramBotClient.SendTextMessageAsync("All windows on your pc was hided");
    }
}