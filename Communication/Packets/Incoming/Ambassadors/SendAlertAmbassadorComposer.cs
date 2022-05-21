using Plus.HabboHotel.GameClients;
using System.Threading.Tasks;
using Plus.Communication.Packets.Outgoing.Rooms.Notifications;
using Plus.HabboHotel.Ambassadors;

namespace Plus.Communication.Packets.Incoming.Ambassadors
{
    internal class AmbassadorSendAlertEvent : IPacketEvent
    {
        private readonly IAmbassadorsManager _ambassadorsManager;

        public AmbassadorSendAlertEvent(IAmbassadorsManager ambassadorsManager) => _ambassadorsManager = ambassadorsManager;

        public Task Parse(GameClient session, ClientPacket packet)
        {
            if (!session.GetHabbo().IsAmbassador)
                return Task.CompletedTask;

            var userid = packet.PopInt();
            var habbo = PlusEnvironment.GetHabboById(userid);

            if(habbo == null)
                return Task.CompletedTask;

            _ambassadorsManager.AddLogs(session.GetHabbo().Id, habbo.Username, "Alert");
            habbo.GetClient().SendPacket(new RoomNotificationComposer("ambassador.alert.warning", "message", "${notification.ambassador.alert.warning.message}"));
            return Task.CompletedTask;
        }
    }
}
