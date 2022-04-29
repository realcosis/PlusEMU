using System;
using System.Data;
using System.Threading.Tasks;
using Plus.Communication.Packets.Outgoing.Inventory.Furni;
using Plus.Communication.Packets.Outgoing.Marketplace;
using Plus.Database;
using Plus.HabboHotel.Catalog.Marketplace;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Incoming.Marketplace;

internal class CancelOfferEvent : IPacketEvent
{
    private readonly IMarketplaceManager _marketplaceManager;

    public CancelOfferEvent(IMarketplaceManager marketplaceManager)
    {
        _marketplaceManager = marketplaceManager;
    }
    public async Task Parse(GameClient session, ClientPacket packet)
    {
        var offerId = packet.PopInt();
        var success = await _marketplaceManager.TryCancelOffer(session.GetHabbo(), offerId);
        session.SendPacket(new MarketplaceCancelOfferResultComposer(offerId, success));
    }
}