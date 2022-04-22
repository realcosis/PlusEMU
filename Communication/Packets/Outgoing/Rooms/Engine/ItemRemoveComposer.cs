using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

internal class ItemRemoveComposer : ServerPacket
{
    public ItemRemoveComposer(Item item, int userId)
        : base(ServerPacketHeader.ItemRemoveMessageComposer)
    {
        WriteString(item.Id.ToString());
        WriteBoolean(false);
        WriteInteger(userId);
    }
}