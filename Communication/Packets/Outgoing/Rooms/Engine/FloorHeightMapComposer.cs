using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

public class FloorHeightMapComposer : IServerPacket
{
    private bool _zoomIn;
    private readonly string _map;
    private readonly int _wallHeight;
    public uint MessageId => ServerPacketHeader.FloorHeightMapComposer;

    public FloorHeightMapComposer(string map, int wallHeight, bool zoomIn = true)
    {
        _map = map;
        _wallHeight = wallHeight;
        _zoomIn = zoomIn;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteBoolean(_zoomIn);
        packet.WriteInteger(_wallHeight);
        packet.WriteString(_map);
    }
}