using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Marketplace;

public class MarketplaceCanMakeOfferResultComposer : IServerPacket
{
    private readonly int _result;
    public uint MessageId => ServerPacketHeader.MarketplaceCanMakeOfferResultComposer;

    public MarketplaceCanMakeOfferResultComposer(int result)
    {
        _result = result;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_result);
        packet.WriteInteger(0);
    }
}