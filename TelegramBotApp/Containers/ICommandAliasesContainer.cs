using TelegramBotApp.Commands;

namespace TelegramBotApp.Containers;

public interface ICommandAliasesContainer
{
    public Task<CommandBlank?> GetAliasCommandAsync(string alias, CancellationToken cancellationToken = default);
    
    public Task<bool> TryAddAliasCommandAsync(string alias, CommandBlank commandBlank, CancellationToken cancellationToken = default);

    public Task<bool> TryRemoveAliasCommandAsync(string alias, CancellationToken cancellationToken = default);
}
