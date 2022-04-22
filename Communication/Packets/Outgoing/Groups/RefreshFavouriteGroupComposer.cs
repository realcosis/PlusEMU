namespace Plus.Communication.Packets.Outgoing.Groups
{
    class RefreshFavouriteGroupComposer : ServerPacket
    {
        public RefreshFavouriteGroupComposer(int id)
            : base(ServerPacketHeader.RefreshFavouriteGroupMessageComposer)
        {
            WriteInteger(id);
        }
    }
}
