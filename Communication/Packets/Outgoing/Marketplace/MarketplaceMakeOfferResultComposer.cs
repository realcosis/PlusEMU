namespace Plus.Communication.Packets.Outgoing.Marketplace
{
    class MarketplaceMakeOfferResultComposer : ServerPacket
    {
        public MarketplaceMakeOfferResultComposer(int success)
            : base(ServerPacketHeader.MarketplaceMakeOfferResultMessageComposer)
        {
            WriteInteger(success);
        }
    }
}
