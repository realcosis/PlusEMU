namespace Plus.Communication.Packets.Outgoing.Groups
{
    class UnknownGroupComposer : ServerPacket
    {
        public UnknownGroupComposer(int groupId, int habboId)
            : base(ServerPacketHeader.UnknownGroupMessageComposer)
        {
            WriteInteger(groupId);
            WriteInteger(habboId);
        }
    }
}