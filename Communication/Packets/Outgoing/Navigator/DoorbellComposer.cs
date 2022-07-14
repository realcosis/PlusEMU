using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Navigator;

internal class DoorbellComposer : IServerPacket
{
    private readonly string _username;

    public int MessageId => ServerPacketHeader.DoorbellMessageComposer;

    public DoorbellComposer(string username)
    {
        _username = username;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteString(_username);
}