using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Rooms.Avatar;

internal class SetChatStylePreferenceEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        var chatBubbleId = packet.ReadInt();

        session.GetHabbo().CustomBubbleId = chatBubbleId;
        session.GetHabbo().SaveChatBubble(chatBubbleId.ToString());

        return Task.CompletedTask;
    }
}