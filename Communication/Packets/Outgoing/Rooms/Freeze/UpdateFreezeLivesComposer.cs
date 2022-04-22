namespace Plus.Communication.Packets.Outgoing.Rooms.Freeze
{
    class UpdateFreezeLivesComposer : ServerPacket
    {
        public UpdateFreezeLivesComposer(int userId, int freezeLives)
            : base(ServerPacketHeader.UpdateFreezeLivesMessageComposer)
        {
            WriteInteger(userId);
            WriteInteger(freezeLives);
        }
    }
}
