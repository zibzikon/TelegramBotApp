using TelegramBotApp.Commands.Arguments;

namespace TelegramBotApp.Commands;

public record CommandBlank(string CommandText, IEnumerable<ICommandArgument> CommandArguments);