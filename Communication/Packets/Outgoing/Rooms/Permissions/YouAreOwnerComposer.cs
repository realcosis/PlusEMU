using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Permissions;

internal class YouAreOwnerComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.YouAreOwnerMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}