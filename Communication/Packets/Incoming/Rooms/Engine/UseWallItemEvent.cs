using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items.Wired;
using Plus.HabboHotel.Quests;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Incoming.Rooms.Engine;

internal class UseWallItemEvent : IPacketEvent
{
    private readonly IRoomManager _roomManager;
    private readonly IQuestManager _questManager;

    public UseWallItemEvent(IRoomManager roomManager, IQuestManager questManager)
    {
        _roomManager = roomManager;
        _questManager = questManager;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session == null || session.GetHabbo() == null || !session.GetHabbo().InRoom)
            return;
        if (!_roomManager.TryGetRoom(session.GetHabbo().CurrentRoomId, out var room))
            return;
        var itemId = packet.PopInt();
        var item = room.GetRoomItemHandler().GetItem(itemId);
        if (item == null)
            return;
        var hasRights = room.CheckRights(session, false, true);
        var request = packet.PopInt();
        item.Interactor.OnTrigger(session, item, request, hasRights);
        item.GetRoom().GetWired().TriggerEvent(WiredBoxType.TriggerStateChanges, session.GetHabbo(), item);
        _questManager.ProgressUserQuest(session, QuestType.ExploreFindItem, item.GetBaseItem().Id);
    }
}