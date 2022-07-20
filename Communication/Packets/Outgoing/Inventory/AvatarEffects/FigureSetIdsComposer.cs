using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Clothing.Parts;

namespace Plus.Communication.Packets.Outgoing.Inventory.AvatarEffects;

public class FigureSetIdsComposer : IServerPacket
{
    private readonly ICollection<ClothingParts> _clothingParts;
    public uint MessageId => ServerPacketHeader.FigureSetIdsComposer;

    public FigureSetIdsComposer(ICollection<ClothingParts> clothingParts)
    {
        _clothingParts = clothingParts;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_clothingParts.Count);
        foreach (var part in _clothingParts.ToList())
            packet.WriteInteger(part.PartId);
        packet.WriteInteger(_clothingParts.Count);
        foreach (var part in _clothingParts.ToList())
            packet.WriteString(part.Part);

    }
}