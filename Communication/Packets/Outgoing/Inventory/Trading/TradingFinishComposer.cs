using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Trading;

internal class TradingFinishComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.TradingFinishMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}