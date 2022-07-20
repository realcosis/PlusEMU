using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Rooms;
using Plus.Utilities;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

public class ObjectsComposer : IServerPacket
{
    private readonly Item[] _objects;
    private readonly Room _room;
    public uint MessageId => ServerPacketHeader.ObjectsComposer;

    public ObjectsComposer(Item[] objects, Room room)
    {
        _objects = objects;
        _room = room;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(1);
        packet.WriteInteger(_room.OwnerId);
        packet.WriteString(_room.OwnerName);
        packet.WriteInteger(_objects.Length);
        foreach (var item in _objects)
            WriteFloorItem(packet, item, Convert.ToInt32(item.UserId));

    }

    private void WriteFloorItem(IOutgoingPacket packet, Item item, int userId)
    {
        packet.WriteInteger(item.Id);
        packet.WriteInteger(item.GetBaseItem().SpriteId);
        packet.WriteInteger(item.GetX);
        packet.WriteInteger(item.GetY);
        packet.WriteInteger(item.Rotation);
        packet.WriteString(TextHandling.GetString(item.GetZ));
        packet.WriteString(string.Empty);
        if (item.LimitedNo > 0)
        {
            packet.WriteInteger(1);
            packet.WriteInteger(256);
            packet.WriteString(item.ExtraData);
            packet.WriteInteger(item.LimitedNo);
            packet.WriteInteger(item.LimitedTot);
        }
        else
            ItemBehaviourUtility.GenerateExtradata(item, packet);
        packet.WriteInteger(-1); // to-do: check
        packet.WriteInteger(item.GetBaseItem().Modes > 1 ? 1 : 0);
        packet.WriteInteger(userId);
    }
}