using System;
using System.Collections.Generic;
using Plus.HabboHotel.Catalog.Utilities;

namespace Plus.HabboHotel.Rooms.AI.Responses
{
    public class BotResponse
    {
        public BotAiType AiType { get; set; }
        public List<string> Keywords { get; set; }
        public string ResponseText { get; set; }
        public string ResponseType { get; set; }
        public List<int> BeverageIds { get; private set; }

        public BotResponse(string botAi, string keywords, string responseText, string responseMode, string responseBeverages)
        {
            AiType = BotUtility.GetAiFromString(botAi);
           
            this.Keywords = new List<string>();
            foreach (var keyword in keywords.Split(','))
            {
                this.Keywords.Add(keyword.ToLower());
            }

            this.ResponseText = responseText;
            ResponseType = responseMode;

            BeverageIds = new List<int>(); 
            if (responseBeverages.Contains(","))
            {
                foreach (var vendingId in responseBeverages.Split(','))
                {
                    try
                    {
                        BeverageIds.Add(int.Parse(vendingId));
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            else if (!String.IsNullOrEmpty(responseBeverages) && (int.Parse(responseBeverages)) > 0)
                BeverageIds.Add(int.Parse(responseBeverages));
        }

        public bool KeywordMatched(string message)
        { 
            foreach (var keyword in Keywords)
            {
                if (message.ToLower().Contains(keyword.ToLower()))
                    return true;
            }
            return false;
        }
    }
}