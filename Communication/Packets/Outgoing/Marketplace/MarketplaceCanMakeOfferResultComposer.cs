using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Marketplace;

internal class MarketplaceCanMakeOfferResultComposer : IServerPacket
{
    private readonly int _result;
    public int MessageId => ServerPacketHeader.MarketplaceCanMakeOfferResultMessageComposer;

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