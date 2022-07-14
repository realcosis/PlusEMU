using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Session;

internal class FlatAccessibleComposer : IServerPacket
{
    private readonly string _username;

    public int MessageId => ServerPacketHeader.FlatAccessibleMessageComposer;

    public FlatAccessibleComposer(string username)
    {
        _username = username;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteString(_username);
}