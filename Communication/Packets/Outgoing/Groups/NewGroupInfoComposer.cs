using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Groups;

public class NewGroupInfoComposer : IServerPacket
{
    private readonly int _roomId;
    private readonly int _groupId;

    public uint MessageId => ServerPacketHeader.NewGroupInfoComposer;

    public NewGroupInfoComposer(int roomId, int groupId)
    {
        _roomId = roomId;
        _groupId = groupId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_roomId);
        packet.WriteInteger(_groupId);
    }
}