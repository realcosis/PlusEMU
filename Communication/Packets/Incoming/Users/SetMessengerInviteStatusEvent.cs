using Plus.HabboHotel.GameClients;
using Plus.Utilities;

namespace Plus.Communication.Packets.Incoming.Users;

internal class SetMessengerInviteStatusEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        var allowMessengerInvites = packet.PopBoolean();
        session.GetHabbo().AllowMessengerInvites = allowMessengerInvites;
        using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
        dbClient.SetQuery("UPDATE `users` SET `ignore_invites` = @ignoreInvites WHERE `id` = @habboId LIMIT 1");
        dbClient.AddParameter("habboId", session.GetHabbo().Id);
        dbClient.AddParameter("ignoreInvites", ConvertExtensions.ToStringEnumValue(allowMessengerInvites));
        dbClient.RunQuery();
    }
}