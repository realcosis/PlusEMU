using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Trading;

public class TradingErrorComposer : IServerPacket
{
    private readonly int _error;
    private readonly string _username;
    public uint MessageId => ServerPacketHeader.TradingErrorComposer;

    public TradingErrorComposer(int error, string username)
    {
        _error = error;
        _username = username;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_error);
        packet.WriteString(_username);
    }
}