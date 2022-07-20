using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Navigator;

public class RoomInfoUpdatedComposer : IServerPacket
{
    private readonly int _roomId;

    public uint MessageId => ServerPacketHeader.RoomInfoUpdatedComposer;

    public RoomInfoUpdatedComposer(int roomId)
    {
        _roomId = roomId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_roomId);
    }
}