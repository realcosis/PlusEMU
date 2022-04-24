using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Moderation;

internal class ModerationMsgEvent : IPacketEvent
{
    private readonly IGameClientManager _clientManager;

    public ModerationMsgEvent(IGameClientManager clientManager)
    {
        _clientManager = clientManager;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!session.GetHabbo().GetPermissions().HasRight("mod_alert"))
            return;
        var userId = packet.PopInt();
        var message = packet.PopString();
        var client = _clientManager.GetClientByUserId(userId);
        if (client == null)
            return;
        client.SendNotification(message);
    }
}