using System.Threading.Tasks;
using Plus.Communication.Packets.Outgoing.Navigator;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Navigator;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Incoming.Navigator;

internal class UpdateNavigatorSettingsEvent : IPacketEvent
{
    INavigatorManager _navigatorManager;

    public UpdateNavigatorSettingsEvent(NavigatorManager navigatorManager)
    {
        _navigatorManager = navigatorManager;
    }

    public Task Parse(GameClient session, ClientPacket packet)
    {
        var roomId = packet.PopInt();
        if (roomId == 0)
            return Task.CompletedTask;
        if (!RoomFactory.TryGetData(roomId, out var _))
            return Task.CompletedTask;
        session.GetHabbo().HomeRoom = roomId;
        _navigatorManager.SaveHomeRoom(session.GetHabbo().Id, roomId);
        session.SendPacket(new NavigatorSettingsComposer(roomId));
        return Task.CompletedTask;
    }
}