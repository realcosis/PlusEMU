using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Quests;

internal class QuestCompletedCompser : IServerPacket
{
    public int MessageId => ServerPacketHeader.QuestCompletedMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}