using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;
using Plus.Utilities;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

internal class ObjectUpdateComposer : IServerPacket
{
    private readonly Item _item;
    private readonly int _userId;
    public int MessageId => ServerPacketHeader.ObjectUpdateMessageComposer;

    public ObjectUpdateComposer(Item item, int userId)
    {
        _item = item;
        _userId = userId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_item.Id);
        packet.WriteInteger(_item.GetBaseItem().SpriteId);
        packet.WriteInteger(_item.GetX);
        packet.WriteInteger(_item.GetY);
        packet.WriteInteger(_item.Rotation);
        packet.WriteString(TextHandling.GetString(_item.GetZ));
        packet.WriteString(string.Empty);
        if (_item.LimitedNo > 0)
        {
            packet.WriteInteger(1);
            packet.WriteInteger(256);
            packet.WriteString(_item.ExtraData);
            packet.WriteInteger(_item.LimitedNo);
            packet.WriteInteger(_item.LimitedTot);
        }
        else
            ItemBehaviourUtility.GenerateExtradata(_item, packet);
        packet.WriteInteger(-1); // to-do: check
        packet.WriteInteger(_item.GetBaseItem().Modes > 1 ? 1 : 0);
        packet.WriteInteger(_userId);

    }
}