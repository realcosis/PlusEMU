using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Trading;

internal class TradingClosedComposer : IServerPacket
{
    private readonly int _userId;
    public int MessageId => ServerPacketHeader.TradingClosedMessageComposer;

    public TradingClosedComposer(int userId)
    {
        _userId = userId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_userId);
        packet.WriteInteger(0);
    }
}