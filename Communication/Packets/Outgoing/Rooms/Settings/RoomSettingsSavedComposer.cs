using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Settings;

public class RoomSettingsSavedComposer : IServerPacket
{
    private readonly int _roomId;

    public uint MessageId => ServerPacketHeader.RoomSettingsSavedComposer;

    public RoomSettingsSavedComposer(int roomId)
    {
        _roomId = roomId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_roomId);
    }
}