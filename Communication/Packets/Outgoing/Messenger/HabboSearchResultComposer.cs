using System.Linq;
using System.Collections.Generic;

using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Outgoing.Messenger
{
    class HabboSearchResultComposer : ServerPacket
    {
        public HabboSearchResultComposer(List<SearchResult> friends, List<SearchResult> otherUsers)
            : base(ServerPacketHeader.HabboSearchResultMessageComposer)
        {
            WriteInteger(friends.Count);
            foreach (SearchResult friend in friends.ToList())
            {
                bool online = (PlusEnvironment.GetGame().GetClientManager().GetClientByUserId(friend.UserId) != null);

                WriteInteger(friend.UserId);
               WriteString(friend.Username);
               WriteString(friend.Motto);
                WriteBoolean(online);
                WriteBoolean(false);
               WriteString(string.Empty);
                WriteInteger(0);
               WriteString(online ? friend.Figure : "");
               WriteString(friend.LastOnline);
            }

            WriteInteger(otherUsers.Count);
            foreach (SearchResult otherUser in otherUsers.ToList())
            {
                bool online = (PlusEnvironment.GetGame().GetClientManager().GetClientByUserId(otherUser.UserId) != null);

                WriteInteger(otherUser.UserId);
               WriteString(otherUser.Username);
               WriteString(otherUser.Motto);
                WriteBoolean(online);
                WriteBoolean(false);
               WriteString(string.Empty);
                WriteInteger(0);
               WriteString(online ? otherUser.Figure : "");
               WriteString(otherUser.LastOnline);
            }
        }
    }
}
