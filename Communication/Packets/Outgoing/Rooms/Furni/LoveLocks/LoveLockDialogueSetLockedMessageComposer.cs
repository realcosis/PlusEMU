namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.LoveLocks
{
    class LoveLockDialogueSetLockedMessageComposer : ServerPacket
    {
        public LoveLockDialogueSetLockedMessageComposer(int itemId)
            : base(ServerPacketHeader.LoveLockDialogueSetLockedMessageComposer)
        {
            WriteInteger(itemId);
        }
    }
}
