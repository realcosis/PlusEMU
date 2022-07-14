using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Marketplace;

internal class MarketplaceCancelOfferResultComposer : IServerPacket
{
    private readonly int _offerId;
    private readonly bool _success;
    public int MessageId => ServerPacketHeader.MarketplaceCancelOfferResultMessageComposer;

    public MarketplaceCancelOfferResultComposer(int offerId, bool success)
    {
        _offerId = offerId;
        _success = success;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_offerId);
        packet.WriteBoolean(_success);
    }
}