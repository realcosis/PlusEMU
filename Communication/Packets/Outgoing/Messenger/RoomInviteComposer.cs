namespace Plus.Communication.Packets.Outgoing.Messenger
{
    class RoomInviteComposer : ServerPacket
    {
        public RoomInviteComposer(int senderId, string text)
            : base(ServerPacketHeader.RoomInviteMessageComposer)
        {
            WriteInteger(senderId);
           WriteString(text);
        }
    }
}
