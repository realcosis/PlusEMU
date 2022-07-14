using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Session;

internal class CloseConnectionComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.CloseConnectionMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}