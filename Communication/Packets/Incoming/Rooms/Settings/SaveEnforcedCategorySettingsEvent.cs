using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Navigator;

namespace Plus.Communication.Packets.Incoming.Rooms.Settings;

internal class SaveEnforcedCategorySettingsEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!PlusEnvironment.GetGame().GetRoomManager().TryGetRoom(packet.PopInt(), out var room))
            return;
        if (!room.CheckRights(session, true))
            return;
        var categoryId = packet.PopInt();
        var tradeSettings = packet.PopInt();
        if (tradeSettings < 0 || tradeSettings > 2)
            tradeSettings = 0;
        if (!PlusEnvironment.GetGame().GetNavigator().TryGetSearchResultList(categoryId, out var searchResultList)) categoryId = 36;
        if (searchResultList.CategoryType != NavigatorCategoryType.Category || searchResultList.RequiredRank > session.GetHabbo().Rank) categoryId = 36;
    }
}