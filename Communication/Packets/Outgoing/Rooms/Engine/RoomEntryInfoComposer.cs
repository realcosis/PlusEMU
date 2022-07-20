using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

public class RoomEntryInfoComposer : IServerPacket
{
    private readonly int _roomId;
    private readonly bool _isOwner;

    public uint MessageId => ServerPacketHeader.RoomEntryInfoComposer;

    public RoomEntryInfoComposer(int roomId, bool isOwner)
    {
        _roomId = roomId;
        _isOwner = isOwner;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_roomId);
        packet.WriteBoolean(_isOwner);
    }
}