using TelegramBotApp.Adapters;
using TelegramBotApp.Commands;

namespace TelegramBotApp.Containers;

public class CommandsContainer : ICommandsContainer
{
    private readonly ICommandAliasesContainer _commandAliasesContainer;

    public CommandsContainer(ICommandAliasesContainer commandAliasesContainer)
    {
        _commandAliasesContainer = commandAliasesContainer;
    }
    
    public bool TryGetCommandByTextMessage(string text, out ICommand command)
    {
        switch (text)
        {
            case "/start": command = new StartCommand(); break;
            case "/shdpc": command = new ShutdownPcCommand(); break;
            case "/restpc": command = new RestartPcCommand(); break;
            case "/hidepcwindows": command = new HideAllWindowsCommand(); break;
            case "/openf": command = new OpenFileCommand(); break;
            case "/openp": command = new OpenFileByPathCommand(); break;
            case "/alicr": command = new CreateAliasCommand(_commandAliasesContainer); break;
            case "/aliex": command = new ExecuteAliasCommandCommand(_commandAliasesContainer, commandsContainer: this); break;
            case "/alidel": command = new DeleteAliasCommand(_commandAliasesContainer); break;
            default: command = new UnknownMessageCommand(); return false;
        }

        return true;
    }
}