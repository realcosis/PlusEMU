using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms.Chat.Filter;

namespace Plus.Communication.Packets.Incoming.Messenger;

internal class SendMsgEvent : IPacketEvent
{
    private readonly IWordFilterManager _wordFilterManager;

    public SendMsgEvent(IWordFilterManager wordFilterManager)
    {
        _wordFilterManager = wordFilterManager;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        var userId = packet.PopInt();
        if (userId == 0 || userId == session.GetHabbo().Id)
            return;
        var message = _wordFilterManager.CheckMessage(packet.PopString());
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