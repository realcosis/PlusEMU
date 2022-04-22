namespace Plus.Communication.Packets.Outgoing.Inventory.Trading
{
    class TradingAcceptComposer : ServerPacket
    {
        public TradingAcceptComposer(int userId, bool accept)
            : base(ServerPacketHeader.TradingAcceptMessageComposer)
        {
            WriteInteger(userId);
            WriteInteger(accept ? 1 : 0);
        }
    }
}
