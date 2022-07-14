using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users;
using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Outgoing.Messenger;

internal class BuddyListComposer : IServerPacket
{
    private readonly ICollection<MessengerBuddy> _friends;
    private readonly Habbo _player;
    private readonly int _pages;
    private readonly int _page;
    public int MessageId => ServerPacketHeader.BuddyListMessageComposer;

    public BuddyListComposer(ICollection<MessengerBuddy> friends, Habbo player, int pages, int page)
    {
        _friends = friends;
        _player = player;
        _pages = pages;
        _page = page;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_pages); // Pages
        packet.WriteInteger(_page); // Page
        packet.WriteInteger(_friends.Count);
        foreach (var friend in _friends.ToList())
            friend.Serialize(packet);
    }
}