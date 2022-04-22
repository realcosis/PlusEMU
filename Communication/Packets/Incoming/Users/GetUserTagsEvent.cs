using Plus.Communication.Packets.Outgoing.Users;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Users
{
    class GetUserTagsEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            var userId = packet.PopInt();

            session.SendPacket(new UserTagsComposer(userId));
        }
    }
}
