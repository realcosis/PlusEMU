using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Quests;

internal class QuestAbortedComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.QuestAbortedMessageComposer;

    public void Compose(IOutgoingPacket packet) => packet.WriteBoolean(false);
}