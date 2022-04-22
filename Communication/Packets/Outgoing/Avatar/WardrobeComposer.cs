using System;
using System.Data;
using Plus.Database.Interfaces;

namespace Plus.Communication.Packets.Outgoing.Avatar
{
    class WardrobeComposer : ServerPacket
    {
        public WardrobeComposer(int userId)
            : base(ServerPacketHeader.WardrobeMessageComposer)
        {
            WriteInteger(1);
            using IQueryAdapter dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
            dbClient.SetQuery("SELECT `slot_id`,`look`,`gender` FROM `user_wardrobe` WHERE `user_id` = '" + userId + "'");
            DataTable wardrobeData = dbClient.GetTable();

            if (wardrobeData == null)
                WriteInteger(0);
            else
            {
                WriteInteger(wardrobeData.Rows.Count);
                foreach (DataRow row in wardrobeData.Rows)
                {
                    WriteInteger(Convert.ToInt32(row["slot_id"]));
                    WriteString(Convert.ToString(row["look"]));
                    WriteString(row["gender"].ToString().ToUpper());
                }
            }
        }
    }
}