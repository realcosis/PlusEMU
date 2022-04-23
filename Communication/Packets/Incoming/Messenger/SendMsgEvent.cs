using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms.Chat;

namespace Plus.Communication.Packets.Incoming.Messenger;

internal class SendMsgEvent : IPacketEvent
{
    private readonly IChatManager _chatManager;

    public SendMsgEvent(IChatManager chatManager)
    {
        _chatManager = chatManager;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session == null || session.GetHabbo() == null || session.GetHabbo().GetMessenger() == null)
            return;
        var userId = packet.PopInt();
        if (userId == 0 || userId == session.GetHabbo().Id)
            return;
        var message = _chatManager.GetFilter().CheckMessage(packet.PopString());
        if (string.IsNullOrWhiteSpace(message))
            return;
        if (session.GetHabbo().TimeMuted > 0)
        {
            session.SendNotification("Oops, you're currently muted - you cannot send messages.");
            return;
        }
        session.GetHabbo().GetMessenger().SendInstantMessage(userId, message);
    }
}