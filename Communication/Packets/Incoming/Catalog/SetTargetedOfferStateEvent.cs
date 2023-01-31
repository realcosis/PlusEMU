using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Catalog;

internal class SetTargetedOfferStateEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet) => throw new NotImplementedException();
}
