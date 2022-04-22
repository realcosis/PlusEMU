using System.Linq;
using System.Collections.Generic;

using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Rooms.FloorPlan
{
    class FloorPlanFloorMapComposer : ServerPacket
    {
        public FloorPlanFloorMapComposer(ICollection<Item> items)
            : base(ServerPacketHeader.FloorPlanFloorMapMessageComposer)
        {
            WriteInteger(items.Count);//TODO: Figure this out, it pushes the room coords, but it iterates them, x,y|x,y|x,y|and so on.
            foreach (Item item in items.ToList())
            {
                WriteInteger(item.GetX);
                WriteInteger(item.GetY);
            }
        }
    }
}
