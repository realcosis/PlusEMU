using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Trading;

public class TradingConfirmedComposer : IServerPacket
{
    private readonly int _userId;
    private readonly bool _confirmed;
    public uint MessageId => ServerPacketHeader.TradingConfirmedComposer;

    public TradingConfirmedComposer(int userId, bool confirmed)
    {
        _userId = userId;
        _confirmed = confirmed;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_userId);
        packet.WriteInteger(_confirmed ? 1 : 0);
    }
}