using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Navigator;

internal class RoomInfoUpdatedComposer : IServerPacket
{
    private readonly int _roomId;

    public int MessageId => ServerPacketHeader.RoomInfoUpdatedMessageComposer;

    public RoomInfoUpdatedComposer(int roomId)
    {
        _roomId = roomId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_roomId);
    }
}