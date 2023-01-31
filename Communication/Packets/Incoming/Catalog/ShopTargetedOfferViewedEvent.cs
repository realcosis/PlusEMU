using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Catalog;

internal class ShopTargetedOfferViewedEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet) => throw new NotImplementedException();
}
