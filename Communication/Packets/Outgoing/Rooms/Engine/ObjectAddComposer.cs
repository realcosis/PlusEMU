using Plus.HabboHotel.Items;
using Plus.Utilities;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

internal class ObjectAddComposer : ServerPacket
{
    public ObjectAddComposer(Item item)
        : base(ServerPacketHeader.ObjectAddMessageComposer)
    {
        WriteInteger(item.Id);
        WriteInteger(item.GetBaseItem().SpriteId);
        WriteInteger(item.GetX);
        WriteInteger(item.GetY);
        WriteInteger(item.Rotation);
        WriteString(TextHandling.GetString(item.GetZ));
        WriteString(string.Empty);
        if (item.LimitedNo > 0)
        {
            WriteInteger(1);
            WriteInteger(256);
            WriteString(item.ExtraData);
            WriteInteger(item.LimitedNo);
            WriteInteger(item.LimitedTot);
        }
        else
            ItemBehaviourUtility.GenerateExtradata(item, this);
        WriteInteger(-1); // to-do: check
        WriteInteger(item.GetBaseItem().Modes > 1 ? 1 : 0);
        WriteInteger(item.UserId);
        WriteString(item.Username);
    }
}