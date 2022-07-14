using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Catalog;


// TODO @80O: Implement
public class VoucherRedeemOkComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.VoucherRedeemOkMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteString(""); //productName
        packet.WriteString(""); //productDescription
    }
}