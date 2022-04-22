namespace Plus.Communication.Packets.Outgoing.Rooms.Furni
{
    class GnomeBoxComposer : ServerPacket
    {
        public GnomeBoxComposer(int itemId)
            : base(ServerPacketHeader.GnomeBoxMessageComposer)
        {
            WriteInteger(itemId);
        }
    }
}
