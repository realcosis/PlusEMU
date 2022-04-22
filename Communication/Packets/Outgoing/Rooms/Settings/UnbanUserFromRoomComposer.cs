namespace Plus.Communication.Packets.Outgoing.Rooms.Settings
{
    class UnbanUserFromRoomComposer : ServerPacket
    {
        public UnbanUserFromRoomComposer(int roomId, int userId)
            : base(ServerPacketHeader.UnbanUserFromRoomMessageComposer)
        {
            WriteInteger(roomId);
            WriteInteger(userId);
        }
    }
}
