using Plus.Communication.Packets.Outgoing.Rooms.Furni.Stickys;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Incoming.Rooms.Furni.Stickys;

internal class GetStickyNoteEvent : IPacketEvent
{
    private readonly IRoomManager _roomManager;

    public GetStickyNoteEvent(IRoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        if (!session.GetHabbo().InRoom)
            return Task.CompletedTask;
        if (!_roomManager.TryGetRoom(session.GetHabbo().CurrentRoomId, out var room))
            return Task.CompletedTask;
        var item = room.GetRoomItemHandler().GetItem(packet.ReadInt());
        if (item == null || item.Definition.InteractionType != InteractionType.Postit)
            return Task.CompletedTask;
        session.Send(new StickyNoteComposer(item.Id.ToString(), item.LegacyDataString));
        return Task.CompletedTask;
    }
}