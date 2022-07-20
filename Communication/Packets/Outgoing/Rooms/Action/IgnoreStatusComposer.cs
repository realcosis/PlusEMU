using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Action;

public class IgnoreStatusComposer : IServerPacket
{
    private readonly int _status;
    private readonly string _username;
    public uint MessageId => ServerPacketHeader.IgnoreStatusComposer;

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