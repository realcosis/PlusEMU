using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Quests;

public class QuestCompletedCompser : IServerPacket
{
    public uint MessageId => ServerPacketHeader.QuestCompletedComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}