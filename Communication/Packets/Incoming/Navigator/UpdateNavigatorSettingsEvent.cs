using System.Threading.Tasks;
using Plus.Communication.Packets.Outgoing.Navigator;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Navigator;

namespace Plus.Communication.Packets.Incoming.Navigator;

internal class UpdateNavigatorSettingsEvent : IPacketEvent
{
    private readonly INavigatorManager _navigatorManager;

    public UpdateNavigatorSettingsEvent(NavigatorManager navigatorManager)
    {
        _navigatorManager = navigatorManager;
    }

    public Task Parse(GameClient session, ClientPacket packet)
    {
        var roomId = packet.PopInt();
        _navigatorManager.SaveHomeRoom(session.GetHabbo(), roomId);
        session.SendPacket(new NavigatorSettingsComposer(roomId));
        return Task.CompletedTask;
    }
}