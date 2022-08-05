using Plus.Communication.Packets;
using Plus.HabboHotel.Users.Messenger;

namespace Plus.HabboHotel.GameClients;

public interface IGameClientManager
{
    int Count { get; }
    ICollection<GameClient> GetClients { get; }
    void OnCycle();
    GameClient? GetClientByUserId(uint userId);
    GameClient? GetClientByUsername(string username);
    bool TryGetClient(int clientId, out GameClient client);
    bool UpdateClientUsername(GameClient client, string oldUsername, string newUsername);
    Task<string> GetNameById(uint id);
    IEnumerable<GameClient> GetClientsById(Dictionary<uint, MessengerBuddy>.KeyCollection users);
    void StaffAlert(IServerPacket message, int exclude = 0);
    void ModAlert(string message);
    void DoAdvertisingReport(GameClient reporter, GameClient target);
    void SendPacket(IServerPacket packet, string fuse = "");
    void LogClonesOut(uint userId);
    void RegisterClient(GameClient client, uint userId, string username);
    void UnregisterClient(uint userid, string username);
    void CloseAll();
}