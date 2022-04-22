namespace Plus.Communication.Packets.Outgoing.Rooms.Settings
{
    class FlatControllerAddedComposer : ServerPacket
    {
        public FlatControllerAddedComposer(int roomId, int userId, string username)
            : base(ServerPacketHeader.FlatControllerAddedMessageComposer)
        {
            WriteInteger(roomId);
            WriteInteger(userId);
           WriteString(username);
        }
    }
}
