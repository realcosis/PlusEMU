using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Rooms.FloorPlan;

public class FloorPlanFloorMapComposer : IServerPacket
{
    private readonly ICollection<Item> _items;

    public uint MessageId => ServerPacketHeader.FloorPlanFloorMapComposer;

    public FloorPlanFloorMapComposer(ICollection<Item> items)
    {
        _items = items;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_items.Count); //TODO: Figure this out, it pushes the room coords, but it iterates them, x,y|x,y|x,y|and so on.
        foreach (var item in _items.ToList())
        {
            packet.WriteInteger(item.GetX);
            packet.WriteInteger(item.GetY);
        }
    }
}