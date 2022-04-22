using Plus.HabboHotel.Rooms;
using System;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Rooms.Avatar
{
    class ApplySignEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            var signId = packet.PopInt();
            if (!PlusEnvironment.GetGame().GetRoomManager().TryGetRoom(session.GetHabbo().CurrentRoomId, out var room))
                return;

            var user = room.GetRoomUserManager().GetRoomUserByHabbo(session.GetHabbo().Id);
            if (user == null)
                return;


            user.UnIdle();

            user.SetStatus("sign", Convert.ToString(signId));
            user.UpdateNeeded = true;
            user.SignTime = PlusEnvironment.GetUnixTimestamp() + 5;
        }
    }
}