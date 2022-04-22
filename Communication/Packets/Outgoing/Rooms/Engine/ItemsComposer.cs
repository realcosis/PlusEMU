using Plus.HabboHotel.Items;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

internal class ItemsComposer : ServerPacket
{
    public ItemsComposer(Item[] objects, Room room)
        : base(ServerPacketHeader.ItemsMessageComposer)
    {
        WriteInteger(1);
        WriteInteger(room.OwnerId);
        WriteString(room.OwnerName);
        WriteInteger(objects.Length);
        foreach (var item in objects) WriteWallItem(item, room.OwnerId);
    }

    private void WriteWallItem(Item item, int userId)
    {
        WriteString(item.Id.ToString());
        WriteInteger(item.Data.SpriteId);
        try
        {
            WriteString(item.WallCoord);
        }
        catch
        {
            WriteString("");
        }
        ItemBehaviourUtility.GenerateWallExtradata(item, this);
        WriteInteger(-1);
        WriteInteger(item.Data.Modes > 1 ? 1 : 0);
        WriteInteger(userId);
    }
}