using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Session;

internal class OpenConnectionComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.OpenConnectionMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}