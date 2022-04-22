namespace Plus.Communication.Packets.Outgoing.Inventory.Furni
{
    class FurniListRemoveComposer : ServerPacket
    {
        public FurniListRemoveComposer(int id)
            : base(ServerPacketHeader.FurniListRemoveMessageComposer)
        {
            WriteInteger(id);
        }
    }
}
