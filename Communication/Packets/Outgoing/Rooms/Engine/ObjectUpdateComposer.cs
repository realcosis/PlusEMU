using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;
using Plus.Utilities;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

public class ObjectUpdateComposer : IServerPacket
{
    private readonly Item _item;
    private readonly int _userId;
    public uint MessageId => ServerPacketHeader.ObjectUpdateComposer;

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
        if (_item.UniqueNumber > 0)
        {
            packet.WriteInteger(1);
            packet.WriteInteger(256);
            packet.WriteString(_item.ExtraData);
            packet.WriteInteger(_item.UniqueNumber);
            packet.WriteInteger(_item.UniqueSeries);
        }
        else
            ItemBehaviourUtility.GenerateExtradata(_item, packet);
        packet.WriteInteger(-1); // to-do: check
        packet.WriteInteger(_item.GetBaseItem().Modes > 1 ? 1 : 0);
        packet.WriteInteger(_userId);

    }
}