using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

public class UserNameChangeComposer : IServerPacket
{
    private readonly int _roomId;
    private readonly int _virtualId;
    private readonly string _username;

    public uint MessageId => ServerPacketHeader.UserNameChangeComposer;

    public UserNameChangeComposer(int roomId, int virtualId, string username)
    {
        _roomId = roomId;
        _virtualId = virtualId;
        _username = username;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_roomId);
        packet.WriteInteger(_virtualId);
        packet.WriteString(_username);
    }
}