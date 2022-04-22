using System.Collections.Generic;
using Plus.HabboHotel.GameClients;
using Plus.Communication.Packets.Outgoing.Navigator;
using Plus.HabboHotel.Navigator;

namespace Plus.Communication.Packets.Incoming.Navigator
{
    class GetNavigatorFlatsEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            var categories = PlusEnvironment.GetGame().GetNavigator().GetEventCategories();

            session.SendPacket(new NavigatorFlatCatsComposer(categories));
        }
    }
}