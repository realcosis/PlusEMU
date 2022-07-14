using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Action;

internal class IgnoreStatusComposer : IServerPacket
{
    private readonly int _status;
    private readonly string _username;
    public int MessageId => ServerPacketHeader.IgnoreStatusMessageComposer;

    public IgnoreStatusComposer(int status, string username)
    {
        _status = status;
        _username = username;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_status);
        packet.WriteString(_username);
    }
}