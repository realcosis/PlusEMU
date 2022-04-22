namespace Plus.Communication.Packets.Outgoing.Rooms.Settings
{
    class RoomMuteSettingsComposer : ServerPacket
    {
        public RoomMuteSettingsComposer(bool status)
            : base(ServerPacketHeader.RoomMuteSettingsMessageComposer)
        {
            WriteBoolean(status);
        }
    }
}