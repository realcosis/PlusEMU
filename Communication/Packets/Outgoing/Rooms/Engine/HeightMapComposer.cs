using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

internal class HeightMapComposer : IServerPacket
{
    private readonly string _map;
    public int MessageId => ServerPacketHeader.HeightMapMessageComposer;

    public HeightMapComposer(string map)
    {
        _map = map;
    }

    public void Compose(IOutgoingPacket packet)
    {
        var split = _map.Replace("\n", "").Split('\r');
        packet.WriteInteger(split[0].Length);
        packet.WriteInteger((split.Length - 1) * split[0].Length);
        var x = 0;
        var y = 0;
        for (y = 0; y < split.Length - 1; y++)
        {
            for (x = 0; x < split[0].Length; x++)
            {
                char pos;
                try
                {
                    pos = split[y][x];
                }
                catch
                {
                    pos = 'x';
                }
                if (pos == 'x')
                    packet.WriteShort(-1);
                else
                {
                    var height = 0;
                    if (int.TryParse(pos.ToString(), out height))
                        height = height * 256;
                    else
                        height = (Convert.ToInt32(pos) - 87) * 256;
                    packet.WriteShort((short)height);
                }
            }
        }
    }
}