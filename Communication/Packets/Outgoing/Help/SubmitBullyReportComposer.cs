using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Help;

public class SubmitBullyReportComposer : IServerPacket
{
    private readonly int _result;
    public int MessageId => ServerPacketHeader.SubmitBullyReportMessageComposer;

    public SubmitBullyReportComposer(int result)
    {
        _result = result;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_result);
}