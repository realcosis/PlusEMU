namespace Plus.Communication.Packets.Outgoing.Messenger
{
    class FindFriendsProcessResultComposer : ServerPacket
    {
        public FindFriendsProcessResultComposer(bool found)
            : base(ServerPacketHeader.FindFriendsProcessResultMessageComposer)
        {
            WriteBoolean(found);
        }
    }
}