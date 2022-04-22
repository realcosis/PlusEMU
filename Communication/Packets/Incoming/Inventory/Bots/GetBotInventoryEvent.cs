using Plus.Communication.Packets.Outgoing.Inventory.Bots;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Inventory.Bots
{
    class GetBotInventoryEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (session.GetHabbo().GetInventoryComponent() == null)
                return;

            var bots = session.GetHabbo().GetInventoryComponent().GetBots();
            session.SendPacket(new BotInventoryComposer(bots));
        }
    }
}
