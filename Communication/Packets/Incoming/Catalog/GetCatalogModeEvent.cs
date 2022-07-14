using Plus.Communication.Packets.Outgoing.Catalog;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Catalog;

internal class GetCatalogModeEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        string mode = packet.ReadString();
        session.Send(new CatalogIndexComposer(session, PlusEnvironment.GetGame().GetCatalog().GetPages()));
        return Task.CompletedTask;
    }
}