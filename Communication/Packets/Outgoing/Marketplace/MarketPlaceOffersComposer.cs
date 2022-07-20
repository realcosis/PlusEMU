using Plus.HabboHotel.Catalog.Marketplace;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Marketplace;

public class MarketPlaceOffersComposer : IServerPacket
{
    private readonly Dictionary<int, MarketOffer> _dictionary;
    private readonly Dictionary<int, int> _dictionary2;
    public uint MessageId => ServerPacketHeader.MarketPlaceOffersComposer;

    public MarketPlaceOffersComposer(Dictionary<int, MarketOffer> dictionary, Dictionary<int, int> dictionary2)
        : base()
    {
        _dictionary = dictionary;
        _dictionary2 = dictionary2;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_dictionary.Count);
        foreach (var (_, value) in _dictionary)
        {
            packet.WriteInteger(value.OfferId);
            packet.WriteInteger(1); //State
            packet.WriteInteger(1);
            packet.WriteInteger(value.SpriteId);
            packet.WriteInteger(256);
            packet.WriteString("");
            packet.WriteInteger(value.LimitedNumber);
            packet.WriteInteger(value.LimitedStack);
            packet.WriteInteger(value.TotalPrice);
            packet.WriteInteger(0);
            packet.WriteInteger(PlusEnvironment.GetGame().GetCatalog().GetMarketplace().AvgPriceForSprite(value.SpriteId));
            packet.WriteInteger(_dictionary2[value.SpriteId]);
        }
        packet.WriteInteger(_dictionary.Count); //Item count to show how many were found.
    }
}