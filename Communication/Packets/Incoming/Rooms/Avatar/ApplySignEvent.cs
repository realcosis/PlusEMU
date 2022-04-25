using System;
using Plus.HabboHotel.GameClients;
using Plus.Utilities;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Incoming.Rooms.Avatar;

internal class ApplySignEvent : IPacketEvent
{
    private readonly IRoomManager _roomManager;

    public ApplySignEvent(IRoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        var signId = packet.PopInt();
        if (!_roomManager.TryGetRoom(session.GetHabbo().CurrentRoomId, out var room))
            return;
        var user = room.GetRoomUserManager().GetRoomUserByHabbo(session.GetHabbo().Id);
        if (user == null)
            return;
        user.UnIdle();
        user.SetStatus("sign", Convert.ToString(signId));
        user.UpdateNeeded = true;
        user.SignTime = UnixTimestamp.GetNow() + 5;
    }
}