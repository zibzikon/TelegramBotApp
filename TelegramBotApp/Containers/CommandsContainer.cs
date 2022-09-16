using TelegramBotApp.Adapters;
using TelegramBotApp.Commands;

namespace TelegramBotApp.Containers;

public class CommandsContainer : ICommandsContainer
{
    public bool TryGetCommandByTextMessage(string text, ITelegramBotClientAdapter telegramBotClientAdapter, out ICommand? command)
    {
        command = null;
        
        switch (text)
        {
            case "start": command = new StartCommand(telegramBotClientAdapter); break;
            case "shutdownpc": command = new ShutdownPcCommand(telegramBotClientAdapter); break;
            case "restartpc": command = new RestartPcCommand(telegramBotClientAdapter); break;
            case "hidepcwindows": command = new HideAllWindowsCommand(telegramBotClientAdapter); break;
            case "openfile": command = new OpenFileCommand(telegramBotClientAdapter); break;
            case "openpath": command = new OpenFileByPathCommand(telegramBotClientAdapter); break;
            
            default: return false;
        }

        return true;
    }
}