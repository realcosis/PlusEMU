using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Session;

internal class RoomReadyComposer : IServerPacket
{
    private readonly int _roomId;
    private readonly string _model;

    public int MessageId => ServerPacketHeader.RoomReadyMessageComposer;

    public RoomReadyComposer(int roomId, string model)
    {
        _roomId = roomId;
        _model = model;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteString(_model);
        packet.WriteInteger(_roomId);
    }
}