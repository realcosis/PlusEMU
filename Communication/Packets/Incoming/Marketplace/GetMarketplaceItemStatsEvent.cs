using System;
using System.Data;
using Plus.Communication.Packets.Outgoing.Marketplace;

namespace Plus.Communication.Packets.Incoming.Marketplace
{
    class GetMarketplaceItemStatsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            var itemId = packet.PopInt();
            var spriteId = packet.PopInt();

            DataRow row;
            using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `avgprice` FROM `catalog_marketplace_data` WHERE `sprite` = @SpriteId LIMIT 1");
                dbClient.AddParameter("SpriteId", spriteId);
                row = dbClient.GetRow();
            }

            session.SendPacket(new MarketplaceItemStatsComposer(itemId, spriteId, (row != null ? Convert.ToInt32(row["avgprice"]) : 0)));
        }
    }
}
