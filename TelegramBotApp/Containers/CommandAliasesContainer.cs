using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using TelegramBotApp.Commands;
using TelegramBotApp.Commands.Arguments;
using TelegramBotApp.Serializers;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TelegramBotApp.Containers;

public class CommandAliasesContainer : ICommandAliasesContainer
{
    private readonly IAliasCommandSerializer _aliasCommandSerializer;
    private readonly Dictionary<string, CommandBlank> _aliasCommands;
    
    public CommandAliasesContainer(IAliasCommandSerializer aliasCommandSerializer)
    {
        _aliasCommandSerializer = aliasCommandSerializer;
        _aliasCommands = _aliasCommandSerializer.DeserializeAliasCommandsFileAsync().Result;
    }
    
    public Task<CommandBlank?> GetAliasCommandAsync(string alias, CancellationToken cancellationToken = default)
    {
        var aliasCommand = _aliasCommands.TryGetValue(alias, out var resultCommandString) ? resultCommandString : null;

        return Task.FromResult(aliasCommand);
    }

    public async Task<bool> TryAddAliasCommandAsync(string alias, CommandBlank commandBlank, CancellationToken cancellationToken = default)
    {
        var canBeAdded = _aliasCommands.TryAdd(alias, commandBlank);
        
        if (!canBeAdded) return false;
        
        await _aliasCommandSerializer.SerializeAliasCommandsToFileAsync(_aliasCommands, cancellationToken);
        return cancellationToken.IsCancellationRequested;
    }

    public async Task<bool> TryRemoveAliasCommandAsync(string alias, CancellationToken cancellationToken = default)
    {
        var isRemoved = _aliasCommands.Remove(alias);
        if (isRemoved ==false)
            return false;
        
        await _aliasCommandSerializer.SerializeAliasCommandsToFileAsync(_aliasCommands ,cancellationToken);
        return cancellationToken.IsCancellationRequested;
    }
    

}