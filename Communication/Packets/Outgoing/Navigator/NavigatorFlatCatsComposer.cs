using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Navigator;

namespace Plus.Communication.Packets.Outgoing.Navigator;

internal class NavigatorFlatCatsComposer : IServerPacket
{
    private readonly ICollection<SearchResultList> _categories;
    public int MessageId => ServerPacketHeader.NavigatorFlatCatsMessageComposer;

    public NavigatorFlatCatsComposer(ICollection<SearchResultList> categories)
    {
        _categories = categories;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_categories.Count);
        foreach (var category in _categories.ToList())
        {
            packet.WriteInteger(category.Id);
            packet.WriteString(category.PublicName);
            packet.WriteBoolean(true); // TODO
        }
    }
}