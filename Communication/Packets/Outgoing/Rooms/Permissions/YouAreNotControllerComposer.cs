using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Permissions;

internal class YouAreNotControllerComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.YouAreNotControllerMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}