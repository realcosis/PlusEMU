using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.BuildersClub;

public class BcBorrowedItemsComposer : IServerPacket
{
    public uint MessageId => ServerPacketHeader.BcBorrowedItemsComposer;
    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(0);
}