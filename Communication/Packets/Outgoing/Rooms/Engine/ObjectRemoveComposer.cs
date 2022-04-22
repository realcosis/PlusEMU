using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine
{
    class ObjectRemoveComposer : ServerPacket
    {
        public ObjectRemoveComposer(Item item, int userId)
            : base(ServerPacketHeader.ObjectRemoveMessageComposer)
        {
           WriteString(item.Id.ToString());
            WriteBoolean(false);
            WriteInteger(userId);
            WriteInteger(0);
        }
    }
}