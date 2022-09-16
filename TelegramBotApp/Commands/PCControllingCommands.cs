using System.Diagnostics;
using System.Runtime.InteropServices;
using TelegramBotApp.Adapters;

namespace TelegramBotApp.Commands;
public class ShutdownPcCommand : ICommand
{
    private readonly ITelegramBotClientAdapter _telegramBotClient;

    public ShutdownPcCommand(ITelegramBotClientAdapter telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    public void Execute(CommandArguments? commandArguments)
    {
        _telegramBotClient.SendTextMessageAsync("Your pc shutdown shortly");
        
        Process.Start("shtdown", "/s, /t 0");
    }
}

public class RestartPcCommand : ICommand
{   
    private readonly ITelegramBotClientAdapter _telegramBotClient;

    public RestartPcCommand(ITelegramBotClientAdapter telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    public void Execute(CommandArguments? commandArguments)
    {
        _telegramBotClient.SendTextMessageAsync("Your pc restarts shortly");
        
        Process.Start("shtdown", "/r, /t 0");
    }
}

public class HideAllWindowsCommand : ICommand
{
    private readonly ITelegramBotClientAdapter _telegramBotClient;
    
    const int WmCommand = 0x111;
    const int MinAll = 419;

    public HideAllWindowsCommand(ITelegramBotClientAdapter telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    
    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    
    [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
    static extern IntPtr SendMessage(IntPtr hWnd, Int32 msg, IntPtr wParam, IntPtr lParam);
    
    public async void Execute(CommandArguments? commandArguments)
    {
        IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
        SendMessage(lHwnd, WmCommand, (IntPtr)MinAll, IntPtr.Zero);
        
        await _telegramBotClient.SendTextMessageAsync("All windows on your pc was hided");
    }
}