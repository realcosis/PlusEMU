namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.LoveLocks
{
    class LoveLockDialogueMessageComposer : ServerPacket
    {
        public LoveLockDialogueMessageComposer(int itemId)
            : base(ServerPacketHeader.LoveLockDialogueMessageComposer)
        {
            WriteInteger(itemId);
            WriteBoolean(true);
        }
    }
}
