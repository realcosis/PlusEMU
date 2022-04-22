using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Moderation;

internal class ModerationCautionEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session == null || session.GetHabbo() == null || !session.GetHabbo().GetPermissions().HasRight("mod_caution"))
            return;
        var userId = packet.PopInt();
        var message = packet.PopString();
        var client = PlusEnvironment.GetGame().GetClientManager().GetClientByUserId(userId);
        if (client == null || client.GetHabbo() == null)
            return;
        using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
        {
            dbClient.RunQuery("UPDATE `user_info` SET `cautions` = `cautions` + '1' WHERE `user_id` = '" + client.GetHabbo().Id + "' LIMIT 1");
        }
        client.SendNotification(message);
    }
}