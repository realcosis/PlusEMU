using System.Data;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Avatar;

public class WardrobeComposer : IServerPacket
{
    private readonly int _userId;
    public uint MessageId => ServerPacketHeader.WardrobeComposer;

    public WardrobeComposer(int userId)
    {
        _userId = userId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(1);
        using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
        dbClient.SetQuery("SELECT `slot_id`,`look`,`gender` FROM `user_wardrobe` WHERE `user_id` = '" + _userId + "'");
        var wardrobeData = dbClient.GetTable();
        if (wardrobeData == null)
            packet.WriteInteger(0);
        else
        {
            packet.WriteInteger(wardrobeData.Rows.Count);
            foreach (DataRow row in wardrobeData.Rows)
            {
                packet.WriteInteger(Convert.ToInt32(row["slot_id"]));
                packet.WriteString(Convert.ToString(row["look"]));
                packet.WriteString(row["gender"].ToString().ToUpper());
            }
        }
    }
}