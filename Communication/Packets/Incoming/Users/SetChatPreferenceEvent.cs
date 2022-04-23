using Plus.Database;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Users;

internal class SetChatPreferenceEvent : IPacketEvent
{
    private readonly IDatabase _database;

    public SetChatPreferenceEvent(IDatabase database)
    {
        _database = database;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        var preference = packet.PopBoolean();
        session.GetHabbo().ChatPreference = preference;
        using var dbClient = _database.GetQueryReactor();
        dbClient.SetQuery("UPDATE `users` SET `chat_preference` = @chatPreference WHERE `id` = '" + session.GetHabbo().Id + "' LIMIT 1");
        dbClient.AddParameter("chatPreference", PlusEnvironment.BoolToEnum(preference));
        dbClient.RunQuery();
    }
}