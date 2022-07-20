using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni;

public class OpenGiftComposer : IServerPacket
{
    private readonly ItemData _data;
    private readonly string _text;
    private readonly Item _item;
    private readonly bool _itemIsInRoom;

    public uint MessageId => ServerPacketHeader.OpenGiftComposer;

    public OpenGiftComposer(ItemData data, string text, Item item, bool itemIsInRoom)
    {
        _data = data;
        _text = text;
        _item = item;
        _itemIsInRoom = itemIsInRoom;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteString(_data.Type.ToString());
        packet.WriteInteger(_data.SpriteId);
        packet.WriteString(_data.ItemName);
        packet.WriteInteger(_item.Id);
        packet.WriteString(_data.Type.ToString());
        packet.WriteBoolean(_itemIsInRoom); //Is it in the room?
        packet.WriteString(_text);
    }
}