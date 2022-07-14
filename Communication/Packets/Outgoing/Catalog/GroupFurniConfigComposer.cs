using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;

namespace Plus.Communication.Packets.Outgoing.Catalog;

internal class GroupFurniConfigComposer : IServerPacket
{
    private readonly ICollection<Group> _groups;
    public int MessageId => ServerPacketHeader.GroupFurniConfigMessageComposer;

    public GroupFurniConfigComposer(ICollection<Group> groups)
    {
        _groups = groups;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_groups.Count);
        foreach (var group in _groups)
        {
            packet.WriteInteger(group.Id);
            packet.WriteString(group.Name);
            packet.WriteString(group.Badge);
            packet.WriteString(PlusEnvironment.GetGame().GetGroupManager().GetColourCode(group.Colour1, true)); // TODO @80O: Pass by constructor / attach to Group object
            packet.WriteString(PlusEnvironment.GetGame().GetGroupManager().GetColourCode(group.Colour2, false)); // TODO @80O: Pass by constructor / attach to Group object
            packet.WriteBoolean(false);
            packet.WriteInteger(group.CreatorId);
            packet.WriteBoolean(group.ForumEnabled);
        }
    }
}