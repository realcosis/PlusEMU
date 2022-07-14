using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.Stickys;

internal class StickyNoteComposer : IServerPacket
{
    private readonly string _itemId;
    private readonly string _extradata;

    public int MessageId => ServerPacketHeader.StickyNoteMessageComposer;

    public StickyNoteComposer(string itemId, string extradata)
    {
        _itemId = itemId;
        _extradata = extradata;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteString(_itemId);
        packet.WriteString(_extradata);
    }
}