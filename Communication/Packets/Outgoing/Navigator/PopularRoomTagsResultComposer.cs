using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Navigator;

internal class PopularRoomTagsResultComposer : IServerPacket
{
    private readonly ICollection<KeyValuePair<string, int>> _tags;

    public int MessageId => ServerPacketHeader.PopularRoomTagsResultMessageComposer;

    public PopularRoomTagsResultComposer(ICollection<KeyValuePair<string, int>> tags)
    {
        _tags = tags;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_tags.Count);
        foreach (var tag in _tags)
        {
            packet.WriteString(tag.Key);
            packet.WriteInteger(tag.Value);
        }
    }
}