using System.Threading.Tasks;

namespace Plus.Communication.Rcon.Commands;

public interface IRconCommand
{
    string Key { get; }
    string Parameters { get; }
    string Description { get; }
    Task<bool> TryExecute(string[] parameters);
}