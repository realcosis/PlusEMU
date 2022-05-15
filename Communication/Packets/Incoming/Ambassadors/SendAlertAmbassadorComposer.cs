using Plus.HabboHotel.GameClients;
using System.Threading.Tasks;
using Plus.Communication.Packets.Outgoing.Moderation;
using Plus.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Plus.Communication.Packets.Incoming.Ambassadors
{
    internal class AmbassadorSendAlertEvent : IPacketEvent
    {

        public Task Parse(GameClient session, ClientPacket packet)
        {
            if (!session.GetHabbo().IsAmbassador)
                return Task.CompletedTask;

            var userid = packet.PopInt();
            var habbo = PlusEnvironment.GetHabboById(userid);

            if(habbo == null)
                return Task.CompletedTask;

            habbo.GetClient().SendPacket(new RoomNotificationComposer("ambassador.alert.warning", "message", "${notification.ambassador.alert.warning.message}"));
            return Task.CompletedTask;
        }
    }
}
