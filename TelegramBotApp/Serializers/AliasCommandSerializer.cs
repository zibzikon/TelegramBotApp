using System.Text.Json;
using System.Text.Json.Serialization;
using TelegramBotApp.Commands;
using TelegramBotApp.Commands.Arguments;

namespace TelegramBotApp.Serializers;

public class AliasCommandSerializer : IAliasCommandSerializer
{
    private readonly string _pathToAliasCommandsFile;
    
    public AliasCommandSerializer(string pathToAliasCommandsFile)
    {
        _pathToAliasCommandsFile = pathToAliasCommandsFile;
    }

    private Dictionary<string, CommandArgumentSerializableObject> OriginallyToSerializableDictionary(Dictionary<string, CommandBlank> aliasCommands)
        => _ = aliasCommands.ToDictionary(aliasCommandPair => 
            aliasCommandPair.Key, aliasCommandPair => new CommandArgumentSerializableObject(aliasCommandPair.Value));
    
    private Dictionary<string, CommandBlank> SerializableAliasCommandsToOriginallyDictionary(Dictionary<string, CommandArgumentSerializableObject> serializedAliasCommands)
        => _ = serializedAliasCommands.ToDictionary(aliasCommandPair => 
            aliasCommandPair.Key, aliasCommandPair => aliasCommandPair.Value.BuildCommandBlank());

    public async Task SerializeAliasCommandsToFileAsync(Dictionary<string, CommandBlank> aliasCommands
        ,CancellationToken cancellationToken = default)
    {

        var aliasCommandsSerializable = OriginallyToSerializableDictionary(aliasCommands);
        var stream = File.Create(_pathToAliasCommandsFile);
        await JsonSerializer.SerializeAsync(utf8Json: stream, value: aliasCommandsSerializable, cancellationToken: cancellationToken);
        await stream.DisposeAsync();
    }
    
    public async Task<Dictionary<string, CommandBlank>> DeserializeAliasCommandsFileAsync(CancellationToken cancellationToken = default)
    {
        if (File.Exists(_pathToAliasCommandsFile) == false)
        {
            File.Create(_pathToAliasCommandsFile);
            return new Dictionary<string, CommandBlank>();
        }
        Dictionary<string, CommandArgumentSerializableObject>? deserializedAliasCommands;
        await using var stream = File.OpenRead(_pathToAliasCommandsFile);

        try
        {
            deserializedAliasCommands =
                await JsonSerializer.DeserializeAsync<Dictionary<string, CommandArgumentSerializableObject>>(utf8Json: stream,
                    cancellationToken: cancellationToken);
        }
        finally
        {
            await stream.DisposeAsync();
        }


        return deserializedAliasCommands != null ? SerializableAliasCommandsToOriginallyDictionary(deserializedAliasCommands) :
            new Dictionary<string, CommandBlank>();
    }
}
public class CommandArgumentSerializableObject
{
    public string CommandText { get; set; }
        
    public CommandArgument[] CommandArguments { get; set; }

    [JsonConstructor]
    public CommandArgumentSerializableObject(string CommandText, CommandArgument[] CommandArguments)
    {
        this.CommandText = CommandText;
        this.CommandArguments = CommandArguments;
    }
        
    public CommandArgumentSerializableObject(CommandBlank commandBlank)
    {
        CommandText = commandBlank.CommandText;
        CommandArguments = CreateCommandArguments(commandBlank.CommandArguments);
    }

    private CommandArgument[] CreateCommandArguments(IEnumerable<ICommandArgument> commandArguments) =>
        _ = commandArguments.Select(commandArgument => new CommandArgument(commandArgument.Message)).ToArray();
        
    public CommandBlank BuildCommandBlank ()=> new(CommandText, CommandArguments);
        
}