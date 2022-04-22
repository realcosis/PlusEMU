namespace Plus.Communication.Packets.Outgoing.BuildersClub
{
    class BcBorrowedItemsComposer : ServerPacket
    {
        public BcBorrowedItemsComposer()
            : base(ServerPacketHeader.BcBorrowedItemsMessageComposer)
        {
            WriteInteger(0);
        }
    }
}
