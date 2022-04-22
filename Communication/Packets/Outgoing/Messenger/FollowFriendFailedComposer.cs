namespace Plus.Communication.Packets.Outgoing.Messenger
{
    class FollowFriendFailedComposer : ServerPacket
    {
        public FollowFriendFailedComposer(int errorCode)
            : base(ServerPacketHeader.FollowFriendFailedMessageComposer)
        {
            WriteInteger(errorCode);
        }
    }
}
