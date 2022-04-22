namespace Plus.Communication.Packets.Outgoing.Rooms.AI.Pets
{
    class AddExperiencePointsComposer : ServerPacket
    {
        public AddExperiencePointsComposer(int petId, int virtualId, int amount)
            : base(ServerPacketHeader.AddExperiencePointsMessageComposer)
        {
            WriteInteger(petId);
            WriteInteger(virtualId);
            WriteInteger(amount);
        }
    }
}