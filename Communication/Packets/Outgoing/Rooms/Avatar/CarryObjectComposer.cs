namespace Plus.Communication.Packets.Outgoing.Rooms.Avatar
{
    class CarryObjectComposer : ServerPacket
    {
        public CarryObjectComposer(int virtualId, int itemId)
            : base(ServerPacketHeader.CarryObjectMessageComposer)
        {
            WriteInteger(virtualId);
            WriteInteger(itemId);
        }
    }
}
