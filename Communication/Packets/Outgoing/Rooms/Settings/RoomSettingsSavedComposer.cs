using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Settings;

internal class RoomSettingsSavedComposer : IServerPacket
{
    private readonly int _roomId;

    public int MessageId => ServerPacketHeader.RoomSettingsSavedMessageComposer;

    public RoomSettingsSavedComposer(int roomId)
    {
        _roomId = roomId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_roomId);
    }
}