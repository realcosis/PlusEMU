using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.AI.Pets;

internal class AddExperiencePointsComposer : IServerPacket
{
    private readonly int _petId;
    private readonly int _virtualId;
    private readonly int _amount;
    public int MessageId => ServerPacketHeader.AddExperiencePointsMessageComposer;

    public AddExperiencePointsComposer(int petId, int virtualId, int amount)
    {
        _petId = petId;
        _virtualId = virtualId;
        _amount = amount;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_petId);
        packet.WriteInteger(_virtualId);
        packet.WriteInteger(_amount);
    }
}