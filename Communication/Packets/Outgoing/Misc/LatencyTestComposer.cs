using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Misc;

internal class LatencyTestComposer : IServerPacket
{
    private readonly int _testResponse;
    public int MessageId => ServerPacketHeader.LatencyResponseMessageComposer;

    public LatencyTestComposer(int testResponse)
    {
        _testResponse = testResponse;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_testResponse);
}