namespace Plus.Communication.Packets.Outgoing.Rooms.Engine
{
    class UserRemoveComposer : ServerPacket
    {
        public UserRemoveComposer(int id)
            : base(ServerPacketHeader.UserRemoveMessageComposer)
        {
           WriteString(id.ToString());
        }
    }
}
