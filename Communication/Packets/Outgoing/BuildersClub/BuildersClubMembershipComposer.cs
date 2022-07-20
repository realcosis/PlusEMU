using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.BuildersClub;

public class BuildersClubMembershipComposer : IServerPacket
{
    public uint MessageId => ServerPacketHeader.BuildersClubMembershipComposer;
    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(int.MaxValue);
        packet.WriteInteger(100);
        packet.WriteInteger(0);
        packet.WriteInteger(int.MaxValue);
    }
}