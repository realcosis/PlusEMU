using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plus.Communication.Packets.Outgoing.Messenger;
using Plus.HabboHotel.Friends;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Incoming.Messenger;

internal class MessengerInitEvent : IPacketEvent
{
    private readonly IMessengerDataLoader _messengerDataLoader;

    public MessengerInitEvent(IMessengerDataLoader messengerDataLoader)
    {
        _messengerDataLoader = messengerDataLoader;
    }

    public async Task Parse(GameClient session, ClientPacket packet)
    {
        ICollection<MessengerBuddy> friends = session.GetHabbo().GetMessenger().Friends.Values.ToList();
        session.SendPacket(new MessengerInitComposer());
        var page = 0;
        if (!friends.Any())
            session.SendPacket(new BuddyListComposer(friends, session.GetHabbo(), 1, 0));
        else
        {
            var pages = (friends.Count() - 1) / 500 + 1;
            foreach (ICollection<MessengerBuddy> batch in friends.Chunk(500))
            {
                session.SendPacket(new BuddyListComposer(batch.ToList(), session.GetHabbo(), pages, page));
                page++;
            }
        }

        var messages = await _messengerDataLoader.GetAndDeleteOfflineMessages(session.GetHabbo().Id);
        foreach (var (userId, message) in messages)
            foreach (var (m, s) in message)
                session.SendPacket(new NewConsoleMessageComposer(userId, m, s));
        return;
    }
}