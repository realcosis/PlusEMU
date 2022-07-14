using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Moderation;

internal class MutedComposer : IServerPacket
{
    private readonly double _timeMuted;

    public int MessageId => ServerPacketHeader.MutedMessageComposer;

    public MutedComposer(double timeMuted)
    {
        _timeMuted = timeMuted;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(Convert.ToInt32(_timeMuted));
}