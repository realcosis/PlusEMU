using Plus.Communication.Packets.Outgoing.GameCenter;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.GameCenter;

internal class GetGameListingEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        var games = PlusEnvironment.GetGame().GetGameDataManager().GameData;
        session.SendPacket(new GameListComposer(games));
    }
}