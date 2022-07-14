using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Furni;

internal class FurniListUpdateComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.FurniListUpdateMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}