using Plus.HabboHotel.GameClients;
using Plus.Database;
using Dapper;

namespace Plus.Communication.Packets.Incoming.Preferences;

internal class SetChatPreferenceEvent : IPacketEvent
{
    private readonly IDatabase _database;

    public SetChatPreferenceEvent(IDatabase database)
    {
        _database = database;
    }

    public async Task Parse(GameClient session, IIncomingPacket packet)
    {
        var chatPreference = packet.ReadBool();
        session.GetHabbo().ChatPreference = chatPreference;
        using var connection = _database.Connection();
        await connection.ExecuteAsync("UPDATE users SET chat_preference = @chatPreference WHERE id = @userId LIMIT 1",
            new { chatPreference, userId = session.GetHabbo().Id});
    }
}