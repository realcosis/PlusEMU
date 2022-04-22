using Plus.Communication.Packets.Outgoing.Groups;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Incoming.Rooms.Furni;

internal class GetGroupFurniSettingsEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session == null || session.GetHabbo() == null || !session.GetHabbo().InRoom)
            return;
        var itemId = packet.PopInt();
        var groupId = packet.PopInt();
        var item = session.GetHabbo().CurrentRoom.GetRoomItemHandler().GetItem(itemId);
        if (item == null)
            return;
        if (item.Data.InteractionType != InteractionType.GuildGate)
            return;
        if (!PlusEnvironment.GetGame().GetGroupManager().TryGetGroup(groupId, out var group))
            return;
        session.SendPacket(new GroupFurniSettingsComposer(group, itemId, session.GetHabbo().Id));
        session.SendPacket(new GroupInfoComposer(group, session));
    }
}