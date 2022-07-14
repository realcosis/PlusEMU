using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Trading;

internal class TradingCompleteComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.TradingCompleteMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}