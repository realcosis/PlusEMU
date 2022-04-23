using Plus.Communication.Packets.Outgoing.Navigator;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Navigator;

namespace Plus.Communication.Packets.Incoming.Navigator;

internal class GetNavigatorFlatsEvent : IPacketEvent
{
    private readonly INavigatorManager _navigatorManager;

    public GetNavigatorFlatsEvent(INavigatorManager navigatorManager)
    {
        _navigatorManager = navigatorManager;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        var categories = _navigatorManager.GetEventCategories();
        session.SendPacket(new NavigatorFlatCatsComposer(categories));
    }
}