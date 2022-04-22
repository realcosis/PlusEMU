using Plus.Communication.Packets.Outgoing.GameCenter;

namespace Plus.Communication.Packets.Incoming.GameCenter
{
    class GetGameListingEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            var games = PlusEnvironment.GetGame().GetGameDataManager().GameData;

            session.SendPacket(new GameListComposer(games));
        }
    }
}
