using System.Linq;
using Plus.HabboHotel.Rooms;
using Plus.Communication.Packets.Outgoing.Groups;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Groups
{
    class GetGroupCreationWindowEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (session == null)
                return;

            var rooms = RoomFactory.GetRoomsDataByOwnerSortByName(session.GetHabbo().Id).Where(x => x.Group == null).ToList();

            session.SendPacket(new GroupCreationWindowComposer(rooms));
        }
    }
}
