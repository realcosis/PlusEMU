using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Ambassadors;

namespace Plus.Communication.Packets.Incoming.Ambassadors
{
    internal class AmbassadorSendAlertEvent : IPacketEvent
    {
        private readonly IAmbassadorsManager _ambassadorsManager;

        public AmbassadorSendAlertEvent(IAmbassadorsManager ambassadorsManager) => _ambassadorsManager = ambassadorsManager;

        public async Task Parse(GameClient session, IIncomingPacket packet)
        {
            var userid = packet.ReadInt();
            var target = PlusEnvironment.GetHabboById(userid);

            await _ambassadorsManager.Warn(session.GetHabbo(), target, "Alert");
        }
    }
}
