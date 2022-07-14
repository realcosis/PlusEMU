using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.BuildersClub;

public class BcBorrowedItemsComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.BcBorrowedItemsMessageComposer;
    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(0);
}