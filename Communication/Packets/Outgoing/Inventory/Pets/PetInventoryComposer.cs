using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms.AI;

namespace Plus.Communication.Packets.Outgoing.Inventory.Pets;

internal class PetInventoryComposer : IServerPacket
{
    private readonly ICollection<Pet> _pets;
    public int MessageId => ServerPacketHeader.PetInventoryMessageComposer;

    public PetInventoryComposer(ICollection<Pet> pets)
    {
        _pets = pets;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(1);
        packet.WriteInteger(1);
        packet.WriteInteger(_pets.Count);
        foreach (var pet in _pets.ToList())
        {
            packet.WriteInteger(pet.PetId);
            packet.WriteString(pet.Name);
            packet.WriteInteger(pet.Type);
            packet.WriteInteger(int.Parse(pet.Race));
            packet.WriteString(pet.Color);
            packet.WriteInteger(0);
            packet.WriteInteger(0);
            packet.WriteInteger(0);
        }
    }
}