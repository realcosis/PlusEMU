using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Settings;

internal class RoomMuteSettingsComposer : IServerPacket
{
    private readonly bool _status;

    public int MessageId => ServerPacketHeader.RoomMuteSettingsMessageComposer;

    public RoomMuteSettingsComposer(bool status)
    {
        _status = status;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteBoolean(_status);
    }
}