using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Camera;

internal class ThumbnailStatusComposer : IServerPacket
{
    public uint MessageId => ServerPacketHeader.ThumbnailStatusComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}