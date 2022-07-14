using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

internal class FloorHeightMapComposer : IServerPacket
{
    private readonly string _map;
    private readonly int _wallHeight;
    public int MessageId => ServerPacketHeader.FloorHeightMapMessageComposer;

    public FloorHeightMapComposer(string map, int wallHeight)
    {
        _map = map;
        _wallHeight = wallHeight;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteBoolean(false);
        packet.WriteInteger(_wallHeight);
        packet.WriteString(_map);
    }
}