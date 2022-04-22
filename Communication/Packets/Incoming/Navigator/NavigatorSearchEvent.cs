using System.Collections.Generic;
using Plus.Communication.Packets.Outgoing.Navigator.New;
using Plus.HabboHotel.Navigator;

namespace Plus.Communication.Packets.Incoming.Navigator
{
    class NavigatorSearchEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            var category = packet.PopString();
            var search = packet.PopString();

            ICollection<SearchResultList> categories = new List<SearchResultList>();

            if (!string.IsNullOrEmpty(search))
            {
                if (PlusEnvironment.GetGame().GetNavigator().TryGetSearchResultList(0, out var queryResult))
                {
                    categories.Add(queryResult);
                }
            }
            else
            {
                categories = PlusEnvironment.GetGame().GetNavigator().GetCategorysForSearch(category);
                if (categories.Count == 0)
                {
                    //Are we going in deep?!
                    categories = PlusEnvironment.GetGame().GetNavigator().GetResultByIdentifier(category);
                    if (categories.Count > 0)
                    {
                        session.SendPacket(new NavigatorSearchResultSetComposer(category, search, categories, session, 2, 100));
                        return;
                    }
                }
            }

            session.SendPacket(new NavigatorSearchResultSetComposer(category, search, categories, session));
        }
    }
}
