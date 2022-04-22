using System;
using System.Data;
using System.Collections.Generic;
using Plus.HabboHotel.LandingView.Promotions;
using NLog;

namespace Plus.HabboHotel.LandingView
{
    public class LandingViewManager
    {
        private static readonly ILogger Log = LogManager.GetLogger("Plus.HabboHotel.LandingView.LandingViewManager");

        private Dictionary<int, Promotion> _promotionItems;

        public LandingViewManager()
        {
            _promotionItems = new Dictionary<int, Promotion>();
        }

        public void Init()
        {
            if (_promotionItems.Count > 0)
                _promotionItems.Clear();

            using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `server_landing` ORDER BY `id` DESC");
                var getData = dbClient.GetTable();

                if (getData != null)
                {
                    foreach (DataRow row in getData.Rows)
                    {
                        _promotionItems.Add(Convert.ToInt32(row[0]), new Promotion((int)row[0], row[1].ToString(), row[2].ToString(), row[3].ToString(), Convert.ToInt32(row[4]), row[5].ToString(), row[6].ToString()));
                    }
                }
            }


            Log.Info("Landing View Manager -> LOADED");
        }

        public ICollection<Promotion> GetPromotionItems()
        {
            return _promotionItems.Values;
        }
    }
}