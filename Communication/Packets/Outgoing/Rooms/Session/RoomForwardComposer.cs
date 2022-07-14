using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Session;

public class RoomForwardComposer : IServerPacket
{
    private readonly int _roomId;
    public int MessageId => ServerPacketHeader.RoomForwardMessageComposer;

    public RoomForwardComposer(int roomId)
    {
        _roomId = roomId;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_roomId);
}