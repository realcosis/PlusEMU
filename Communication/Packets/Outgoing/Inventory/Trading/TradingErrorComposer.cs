namespace Plus.Communication.Packets.Outgoing.Inventory.Trading
{
    class TradingErrorComposer : ServerPacket
    {
        public TradingErrorComposer(int error, string username)
            : base(ServerPacketHeader.TradingErrorMessageComposer)
        {
            WriteInteger(error);
           WriteString(username);
        }
    }
}
