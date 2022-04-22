using System;
using Plus.Utilities;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine
{
    class ObjectsComposer : ServerPacket
    {
        public ObjectsComposer(Item[] objects, Room room)
            : base(ServerPacketHeader.ObjectsMessageComposer)
        {
            WriteInteger(1);

            WriteInteger(room.OwnerId);
           WriteString(room.OwnerName);

            WriteInteger(objects.Length);
            foreach (Item item in objects)
            {
                WriteFloorItem(item, Convert.ToInt32(item.UserId));
            }
        }

        private void WriteFloorItem(Item item, int userId)
        {

            WriteInteger(item.Id);
            WriteInteger(item.GetBaseItem().SpriteId);
            WriteInteger(item.GetX);
            WriteInteger(item.GetY);
            WriteInteger(item.Rotation);
           WriteString(String.Format("{0:0.00}", TextHandling.GetString(item.GetZ)));
           WriteString(String.Empty);

            if (item.LimitedNo > 0)
            {
                WriteInteger(1);
                WriteInteger(256);
               WriteString(item.ExtraData);
                WriteInteger(item.LimitedNo);
                WriteInteger(item.LimitedTot);
            }
            else
            {
                ItemBehaviourUtility.GenerateExtradata(item, this);
            }

            WriteInteger(-1); // to-do: check
            WriteInteger((item.GetBaseItem().Modes > 1) ? 1 : 0);
            WriteInteger(userId);
        }
    }
}