namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.Stickys
{
    class StickyNoteComposer : ServerPacket
    {
        public StickyNoteComposer(string itemId, string extradata)
            : base(ServerPacketHeader.StickyNoteMessageComposer)
        {
           WriteString(itemId);
           WriteString(extradata);
        }
    }
}
