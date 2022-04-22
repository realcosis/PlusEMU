namespace Plus.Communication.Packets.Outgoing.Rooms.Session
{
    class RoomReadyComposer : ServerPacket
    {
        public RoomReadyComposer(int roomId, string model)
            : base(ServerPacketHeader.RoomReadyMessageComposer)
        {
           WriteString(model);
            WriteInteger(roomId);
        }
    }
}
