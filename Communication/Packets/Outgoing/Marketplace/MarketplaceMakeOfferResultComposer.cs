using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Marketplace;

internal class MarketplaceMakeOfferResultComposer : IServerPacket
{
    private readonly int _success;
    public int MessageId => ServerPacketHeader.MarketplaceMakeOfferResultMessageComposer;

    public MarketplaceMakeOfferResultComposer(int success)
    {
        _success = success;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_success);
}