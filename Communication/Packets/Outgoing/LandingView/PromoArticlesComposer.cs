using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.LandingView.Promotions;

namespace Plus.Communication.Packets.Outgoing.LandingView;

public class PromoArticlesComposer : IServerPacket
{
    private readonly ICollection<Promotion> _landingPromotions;
    public uint MessageId => ServerPacketHeader.PromoArticlesComposer;

    public PromoArticlesComposer(ICollection<Promotion> landingPromotions)
    {
        _landingPromotions = landingPromotions;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_landingPromotions.Count); //Count
        foreach (var promotion in _landingPromotions.ToList())
        {
            packet.WriteInteger(promotion.Id); //ID
            packet.WriteString(promotion.Title); //Title
            packet.WriteString(promotion.Text); //Text
            packet.WriteString(promotion.ButtonText); //Button text
            packet.WriteInteger(promotion.ButtonType); //Link type 0 and 3
            packet.WriteString(promotion.ButtonLink); //Link to article
            packet.WriteString(promotion.ImageLink); //Image link
        }
    }
}