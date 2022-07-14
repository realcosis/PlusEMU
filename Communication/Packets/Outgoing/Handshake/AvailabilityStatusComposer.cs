using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Handshake;

internal class AvailabilityStatusComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.AvailabilityStatusMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // TODO @80O: Pass variables via constructor.
        packet.WriteBoolean(true);
        packet.WriteBoolean(false);
        packet.WriteBoolean(true);
    }
}