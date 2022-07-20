using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Catalog;

public class VoucherRedeemErrorComposer : IServerPacket
{
    private readonly int _type;
    public uint MessageId => ServerPacketHeader.VoucherRedeemErrorComposer;

    public VoucherRedeemErrorComposer(int type) => _type = type; // TODO @80O: Extract enum with all errors

    public void Compose(IOutgoingPacket packet) => packet.WriteString(_type.ToString());
}