using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

internal class UserRemoveComposer : IServerPacket
{
    private readonly int _id;

    public int MessageId => ServerPacketHeader.UserRemoveMessageComposer;

    public UserRemoveComposer(int id)
    {
        _id = id;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteString(_id.ToString());
}