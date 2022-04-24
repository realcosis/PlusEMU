using Plus.Database;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Users;

internal class SetMessengerInviteStatusEvent : IPacketEvent
{
    private readonly IDatabase _database;

    public SetMessengerInviteStatusEvent(IDatabase database)
    {
        _database = database;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        var status = packet.PopBoolean();
        session.GetHabbo().AllowMessengerInvites = status;
        using var dbClient = _database.GetQueryReactor();
        dbClient.SetQuery("UPDATE `users` SET `ignore_invites` = @MessengerInvites WHERE `id` = '" + session.GetHabbo().Id + "' LIMIT 1");
        dbClient.AddParameter("MessengerInvites", PlusEnvironment.BoolToEnum(status));
        dbClient.RunQuery();
    }
}