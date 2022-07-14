using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Navigator;

internal class FlatCreatedComposer : IServerPacket
{
    private readonly int _roomId;
    private readonly string _roomName;

    public int MessageId => ServerPacketHeader.FlatCreatedMessageComposer;

    public FlatCreatedComposer(int roomId, string roomName)
    {
        _roomId = roomId;
        _roomName = roomName;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_roomId);
        packet.WriteString(_roomName);
    }
}