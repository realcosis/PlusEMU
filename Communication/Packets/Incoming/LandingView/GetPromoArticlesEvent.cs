using Plus.Communication.Packets.Outgoing.LandingView;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.LandingView;

internal class GetPromoArticlesEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        var landingPromotions = PlusEnvironment.GetGame().GetLandingManager().GetPromotionItems();
        session.SendPacket(new PromoArticlesComposer(landingPromotions));
    }
}