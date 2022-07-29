using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni;

public class OpenGiftComposer : IServerPacket
{
    private readonly ItemDefinition _definition;
    private readonly string _text;
    private readonly Item _item;
    private readonly bool _itemIsInRoom;

    public uint MessageId => ServerPacketHeader.OpenGiftComposer;

    public OpenGiftComposer(ItemDefinition definition, string text, Item item, bool itemIsInRoom)
    {
        _definition = definition;
        _text = text;
        _item = item;
        _itemIsInRoom = itemIsInRoom;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteString(_definition.Type.ToString());
        packet.WriteInteger(_definition.SpriteId);
        packet.WriteString(_definition.ItemName);
        packet.WriteInteger(_item.Id);
        packet.WriteString(_definition.Type.ToString());
        packet.WriteBoolean(_itemIsInRoom); //Is it in the room?
        packet.WriteString(_text);
    }
}