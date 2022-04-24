using Plus.Database;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Users;

internal class SetUserFocusPreferenceEvent : IPacketEvent
{
    private readonly IDatabase _database;

    public SetUserFocusPreferenceEvent(IDatabase database)
    {
        _database = database;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        var focusPreference = packet.PopBoolean();
        session.GetHabbo().FocusPreference = focusPreference;
        using var dbClient = _database.GetQueryReactor();
        dbClient.SetQuery("UPDATE `users` SET `focus_preference` = @focusPreference WHERE `id` = '" + session.GetHabbo().Id + "' LIMIT 1");
        dbClient.AddParameter("focusPreference", PlusEnvironment.BoolToEnum(focusPreference));
        dbClient.RunQuery();
    }
}