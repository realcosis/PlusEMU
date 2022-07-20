using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users;

namespace Plus.Communication.Packets.Outgoing.Groups;

public class GroupMembershipRequestedComposer : IServerPacket
{
    private readonly int _groupId;
    private readonly Habbo _habbo;
    private readonly int _type;

    public uint MessageId => ServerPacketHeader.GroupMembershipRequestedComposer;

    public GroupMembershipRequestedComposer(int groupId, Habbo habbo, int type)
    {
        _groupId = groupId;
        _habbo = habbo;
        _type = type;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_groupId); //GroupId
        packet.WriteInteger(_type); //Type?
        {
            packet.WriteInteger(_habbo.Id); //UserId
            packet.WriteString(_habbo.Username);
            packet.WriteString(_habbo.Look);
            packet.WriteString(string.Empty);
        }
    }
}