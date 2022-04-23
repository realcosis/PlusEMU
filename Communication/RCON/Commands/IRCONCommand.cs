namespace Plus.Communication.Rcon.Commands;

public interface IRconCommand
{
    string Key { get; }
    string Parameters { get; }
    string Description { get; }
    bool TryExecute(string[] parameters);
}