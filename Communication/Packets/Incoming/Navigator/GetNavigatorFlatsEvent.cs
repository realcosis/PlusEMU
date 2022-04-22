using Plus.Communication.Packets.Outgoing.Navigator;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Navigator;

internal class GetNavigatorFlatsEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        var categories = PlusEnvironment.GetGame().GetNavigator().GetEventCategories();
        session.SendPacket(new NavigatorFlatCatsComposer(categories));
    }
}