namespace Plus.Communication.Packets.Outgoing.Groups
{
    class NewGroupInfoComposer : ServerPacket
    {
        public NewGroupInfoComposer(int roomId, int groupId)
            : base(ServerPacketHeader.NewGroupInfoMessageComposer)
        {
            WriteInteger(roomId);
            WriteInteger(groupId);
        }
    }
}
