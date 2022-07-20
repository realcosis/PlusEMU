using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Misc;

public class LatencyTestComposer : IServerPacket
{
    private readonly int _testResponse;
    public uint MessageId => ServerPacketHeader.LatencyResponseComposer;

    public LatencyTestComposer(int testResponse)
    {
        _testResponse = testResponse;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_testResponse);
}