using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Catalog;

public class CatalogUpdatedComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.CatalogUpdatedMessageComposer;

    public void Compose(IOutgoingPacket packet) => packet.WriteBoolean(false);
}