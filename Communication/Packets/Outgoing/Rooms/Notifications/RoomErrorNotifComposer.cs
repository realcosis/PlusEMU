using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Notifications;

public class RoomErrorNotifComposer : IServerPacket
{
    private readonly int _error;
    public uint MessageId => ServerPacketHeader.RoomErrorNotifComposer;

    public RoomErrorNotifComposer(int error)
    {
        _error = error;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_error);
}