using System.Linq;
using System.Collections.Generic;

using Plus.Utilities;
using Plus.HabboHotel.Users.Messenger;

using Plus.Communication.Packets.Outgoing.Messenger;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Messenger
{
    class HabboSearchEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (session == null || session.GetHabbo() == null || session.GetHabbo().GetMessenger() == null)
                return;

            var query = StringCharFilter.Escape(packet.PopString().Replace("%", ""));
            if (query.Length < 1 || query.Length > 100)
                return;

            var friends = new List<SearchResult>();
            var othersUsers = new List<SearchResult>();

            var results = SearchResultFactory.GetSearchResult(query);
            foreach (var result in results.ToList())
            {
                if (session.GetHabbo().GetMessenger().FriendshipExists(result.UserId))
                    friends.Add(result);
                else
                    othersUsers.Add(result);
            }

            session.SendPacket(new HabboSearchResultComposer(friends, othersUsers));
        }
    }
}