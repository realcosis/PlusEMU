using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Settings;

internal class FlatControllerAddedComposer : IServerPacket
{
    private readonly int _roomId;
    private readonly int _userId;
    private readonly string _username;
    public int MessageId => ServerPacketHeader.FlatControllerAddedMessageComposer;

    public FlatControllerAddedComposer(int roomId, int userId, string username)
    {
        _roomId = roomId;
        _userId = userId;
        _username = username;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_roomId);
        packet.WriteInteger(_userId);
        packet.WriteString(_username);
    }
}