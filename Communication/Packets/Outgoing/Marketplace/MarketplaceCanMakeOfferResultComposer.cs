namespace Plus.Communication.Packets.Outgoing.Marketplace
{
    class MarketplaceCanMakeOfferResultComposer : ServerPacket
    {
        public MarketplaceCanMakeOfferResultComposer(int result)
            : base(ServerPacketHeader.MarketplaceCanMakeOfferResultMessageComposer)
        {
            WriteInteger(result);
            WriteInteger(0);
        }
    }
}
