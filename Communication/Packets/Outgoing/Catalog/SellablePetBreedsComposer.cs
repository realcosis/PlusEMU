using Plus.HabboHotel.Catalog.Pets;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Catalog;

public class SellablePetBreedsComposer : IServerPacket
{
    private readonly string _petType;
    private readonly int _petId;
    private readonly ICollection<PetRace> _races;
    public int MessageId => ServerPacketHeader.SellablePetBreedsMessageComposer;

    public SellablePetBreedsComposer(string petType, int petId, ICollection<PetRace> races)
    {
        _petType = petType;
        _petId = petId;
        _races = races;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteString(_petType);
        packet.WriteInteger(_races.Count);
        foreach (var race in _races.ToList())
        {
            packet.WriteInteger(_petId);
            packet.WriteInteger(race.PrimaryColour);
            packet.WriteInteger(race.SecondaryColour);
            packet.WriteBoolean(race.HasPrimaryColour);
            packet.WriteBoolean(race.HasSecondaryColour);
        }
    }
}