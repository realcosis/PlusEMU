namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.LoveLocks
{
    class LoveLockDialogueCloseMessageComposer : ServerPacket
    {
        public LoveLockDialogueCloseMessageComposer(int itemId)
            : base(ServerPacketHeader.LoveLockDialogueCloseMessageComposer)
        {
            WriteInteger(itemId);
        }
    }
}
