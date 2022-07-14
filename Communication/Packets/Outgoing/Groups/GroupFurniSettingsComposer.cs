using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;

namespace Plus.Communication.Packets.Outgoing.Groups;

internal class GroupFurniSettingsComposer : IServerPacket
{
    private readonly Group _group;
    private readonly int _itemId;
    private readonly int _userId;
    public int MessageId => ServerPacketHeader.GroupFurniSettingsMessageComposer;

    public GroupFurniSettingsComposer(Group group, int itemId, int userId)
    {
        _group = group;
        _itemId = itemId;
        _userId = userId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_itemId); //Item Id
        packet.WriteInteger(_group.Id); //Group Id?
        packet.WriteString(_group.Name);
        packet.WriteInteger(_group.RoomId); //RoomId
        packet.WriteBoolean(_group.IsMember(_userId)); //Member?
        packet.WriteBoolean(_group.ForumEnabled); //Has a forum
    }
}