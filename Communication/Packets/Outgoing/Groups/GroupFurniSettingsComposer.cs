using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;

namespace Plus.Communication.Packets.Outgoing.Groups;

public class GroupFurniSettingsComposer : IServerPacket
{
    private readonly Group _group;
    private readonly uint _itemId;
    private readonly int _userId;
    public uint MessageId => ServerPacketHeader.GroupFurniSettingsComposer;

    public GroupFurniSettingsComposer(Group group, uint itemId, int userId)
    {
        _group = group;
        _itemId = itemId;
        _userId = userId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteUInteger(_itemId); //Item Id
        packet.WriteInteger(_group.Id); //Group Id?
        packet.WriteString(_group.Name);
        packet.WriteUInteger(_group.RoomId); //RoomId
        packet.WriteBoolean(_group.IsMember(_userId)); //Member?
        packet.WriteBoolean(_group.ForumEnabled); //Has a forum
    }
}