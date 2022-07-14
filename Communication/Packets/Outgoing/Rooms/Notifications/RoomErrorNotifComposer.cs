using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Notifications;

internal class RoomErrorNotifComposer : IServerPacket
{
    private readonly int _error;
    public int MessageId => ServerPacketHeader.RoomErrorNotifMessageComposer;

    public RoomErrorNotifComposer(int error)
    {
        _error = error;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_error);
}