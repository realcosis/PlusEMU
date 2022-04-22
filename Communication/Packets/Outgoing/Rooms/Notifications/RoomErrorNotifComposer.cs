namespace Plus.Communication.Packets.Outgoing.Rooms.Notifications
{
    class RoomErrorNotifComposer : ServerPacket
    {
        public RoomErrorNotifComposer(int error)
            : base(ServerPacketHeader.RoomErrorNotifMessageComposer)
        {
            WriteInteger(error);
        }
    }
}
