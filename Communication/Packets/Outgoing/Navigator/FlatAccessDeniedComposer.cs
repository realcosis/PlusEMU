using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Navigator;

internal class FlatAccessDeniedComposer : IServerPacket
{
    private readonly string _username;
    public int MessageId => ServerPacketHeader.FlatAccessDeniedMessageComposer;

    public FlatAccessDeniedComposer(string username)
    {
        _username = username;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteString(_username);
}