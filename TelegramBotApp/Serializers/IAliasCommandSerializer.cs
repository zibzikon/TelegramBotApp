using TelegramBotApp.Commands;

namespace TelegramBotApp.Serializers;

public interface IAliasCommandSerializer
{
    public Task<Dictionary<string, CommandBlank>> DeserializeAliasCommandsFileAsync(
        CancellationToken cancellationToken = default);

    public Task SerializeAliasCommandsToFileAsync(Dictionary<string, CommandBlank> aliasCommands
        , CancellationToken cancellationToken = default);
}