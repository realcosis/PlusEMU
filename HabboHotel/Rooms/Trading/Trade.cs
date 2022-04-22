using System.Linq;
using Plus.HabboHotel.Items;
using Plus.Communication.Packets.Outgoing.Inventory.Trading;
using Plus.Communication.Packets.Outgoing;
using Plus.Communication.Packets.Outgoing.Moderation;
using Plus.Communication.Packets.Outgoing.Inventory.Furni;
using Plus.Communication.Packets.Outgoing.Inventory.Purse;

namespace Plus.HabboHotel.Rooms.Trading
{
    public sealed class Trade
    {
        public int Id { get; set; }
        public TradeUser[] Users { get; set; }
        public bool CanChange { get; set; }

        private Room _instance = null;

        public Trade(int id, RoomUser playerOne, RoomUser playerTwo, Room room)
        {
            Id = id;
            CanChange = true;
            _instance = room;
            Users = new TradeUser[2];
            Users[0] = new TradeUser(playerOne);
            Users[1] = new TradeUser(playerTwo);

            playerOne.IsTrading = true;
            playerOne.TradeId = Id;
            playerOne.TradePartner = playerTwo.UserId;
            playerTwo.IsTrading = true;
            playerTwo.TradeId = Id;
            playerTwo.TradePartner = playerOne.UserId;
        }

        public void SendPacket(ServerPacket packet)
        {
            foreach (var user in Users)
            {
                if (user == null || user.RoomUser == null || user.RoomUser.GetClient() == null)
                    continue;

                user.RoomUser.GetClient().SendPacket(packet);
            }
        }

        public void RemoveAccepted()
        {
            foreach (var user in Users)
            {
                if (user == null)
                    continue;

                user.HasAccepted = false;
            }
        }

        public bool AllAccepted
        {
            get
            {
                foreach (var user in Users)
                {
                    if (user == null)
                        continue;

                    if (!user.HasAccepted)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void EndTrade(int userId)
        {
            foreach (var tradeUser in Users)
            {
                if (tradeUser == null || tradeUser.RoomUser == null)
                    continue;

                RemoveTrade(tradeUser.RoomUser.UserId);
            }

            SendPacket(new TradingClosedComposer(userId));
            _instance.GetTrading().RemoveTrade(Id);
        }

        public void Finish()
        {
            foreach (var tradeUser in Users)
            {
                if (tradeUser == null)
                    continue;

                RemoveTrade(tradeUser.RoomUser.UserId);
            }

            ProcessItems();
            SendPacket(new TradingFinishComposer());

            _instance.GetTrading().RemoveTrade(Id);
        }

        public void RemoveTrade(int userId)
        {
            var tradeUser = Users[0];

            if (tradeUser.RoomUser.UserId != userId)
            {
                tradeUser = Users[1];
            }

            tradeUser.RoomUser.RemoveStatus("trd");
            tradeUser.RoomUser.UpdateNeeded = true;
            tradeUser.RoomUser.IsTrading = false;
            tradeUser.RoomUser.TradeId = 0;
            tradeUser.RoomUser.TradePartner = 0;
        }

        public void ProcessItems()
        {
            var userOne = Users[0].OfferedItems.Values.ToList();
            var userTwo = Users[1].OfferedItems.Values.ToList();

            var roomUserOne = Users[0].RoomUser;
            var roomUserTwo = Users[1].RoomUser;

            var logUserOne = "";
            var logUserTwo = "";

            if (roomUserOne == null || roomUserOne.GetClient() == null || roomUserOne.GetClient().GetHabbo() == null || roomUserOne.GetClient().GetHabbo().GetInventoryComponent() == null)
                return;

            if (roomUserTwo == null || roomUserTwo.GetClient() == null || roomUserTwo.GetClient().GetHabbo() == null || roomUserTwo.GetClient().GetHabbo().GetInventoryComponent() == null)
                return;

            foreach (var item in userOne)
            {
                var I = roomUserOne.GetClient().GetHabbo().GetInventoryComponent().GetItem(item.Id);

                if (I == null)
                {
                    SendPacket(new BroadcastMessageAlertComposer("Error! Trading Failed!"));
                    return;
                }
            }

            foreach (var item in userTwo)
            {
                var I = roomUserTwo.GetClient().GetHabbo().GetInventoryComponent().GetItem(item.Id);

                if (I == null)
                {
                    SendPacket(new BroadcastMessageAlertComposer("Error! Trading Failed!"));
                    return;
                }
            }
            using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
            foreach (var item in userOne)
            {
                logUserOne += item.Id + ";";
                roomUserOne.GetClient().GetHabbo().GetInventoryComponent().RemoveItem(item.Id);
                if (item.Data.InteractionType == InteractionType.Exchange && PlusEnvironment.GetSettingsManager().TryGetValue("trading.auto_exchange_redeemables") == "1")
                {
                    roomUserTwo.GetClient().GetHabbo().Credits += item.Data.BehaviourData;
                    roomUserTwo.GetClient().SendPacket(new CreditBalanceComposer(roomUserTwo.GetClient().GetHabbo().Credits));

                    dbClient.SetQuery("DELETE FROM `items` WHERE `id` = @id LIMIT 1");
                    dbClient.AddParameter("id", item.Id);
                    dbClient.RunQuery();
                }
                else
                {
                    if (roomUserTwo.GetClient().GetHabbo().GetInventoryComponent().TryAddItem(item))
                    {
                        roomUserTwo.GetClient().SendPacket(new FurniListAddComposer(item));
                        roomUserTwo.GetClient().SendPacket(new FurniListNotificationComposer(item.Id, 1));

                        dbClient.SetQuery("UPDATE `items` SET `user_id` = @user WHERE id=@id LIMIT 1");
                        dbClient.AddParameter("user", roomUserTwo.UserId);
                        dbClient.AddParameter("id", item.Id);
                        dbClient.RunQuery();
                    }
                }
            }

            foreach (var item in userTwo)
            {
                logUserTwo += item.Id + ";";
                roomUserTwo.GetClient().GetHabbo().GetInventoryComponent().RemoveItem(item.Id);
                if (item.Data.InteractionType == InteractionType.Exchange && PlusEnvironment.GetSettingsManager().TryGetValue("trading.auto_exchange_redeemables") == "1")
                {
                    roomUserOne.GetClient().GetHabbo().Credits += item.Data.BehaviourData;
                    roomUserOne.GetClient().SendPacket(new CreditBalanceComposer(roomUserOne.GetClient().GetHabbo().Credits));

                    dbClient.SetQuery("DELETE FROM `items` WHERE `id` = @id LIMIT 1");
                    dbClient.AddParameter("id", item.Id);
                    dbClient.RunQuery();
                }
                else
                {
                    if (roomUserOne.GetClient().GetHabbo().GetInventoryComponent().TryAddItem(item))
                    {
                        roomUserOne.GetClient().SendPacket(new FurniListAddComposer(item));
                        roomUserOne.GetClient().SendPacket(new FurniListNotificationComposer(item.Id, 1));

                        dbClient.SetQuery("UPDATE `items` SET `user_id` = @user WHERE id=@id LIMIT 1");
                        dbClient.AddParameter("user", roomUserOne.UserId);
                        dbClient.AddParameter("id", item.Id);
                        dbClient.RunQuery();
                    }
                }
            }

            dbClient.SetQuery("INSERT INTO `logs_client_trade` VALUES(null, @1id, @2id, @1items, @2items, UNIX_TIMESTAMP())");
            dbClient.AddParameter("1id", roomUserOne.UserId);
            dbClient.AddParameter("2id", roomUserTwo.UserId);
            dbClient.AddParameter("1items", logUserOne);
            dbClient.AddParameter("2items", logUserTwo);
            dbClient.RunQuery();
        }
    }
}