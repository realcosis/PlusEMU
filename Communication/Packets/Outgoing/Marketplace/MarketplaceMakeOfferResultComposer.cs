using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Marketplace;

public class MarketplaceMakeOfferResultComposer : IServerPacket
{
    private readonly int _success;
    public uint MessageId => ServerPacketHeader.MarketplaceMakeOfferResultComposer;

    public MarketplaceMakeOfferResultComposer(int success)
    {
        _success = success;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_success);
}