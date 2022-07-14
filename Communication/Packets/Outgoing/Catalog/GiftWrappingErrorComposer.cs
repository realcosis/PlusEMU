using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Catalog;

internal class GiftWrappingErrorComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.GiftWrappingErrorMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // TODO @80O: Verify empty body?
    }
}