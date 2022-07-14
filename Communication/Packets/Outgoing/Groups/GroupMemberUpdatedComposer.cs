using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users;

namespace Plus.Communication.Packets.Outgoing.Groups;

internal class GroupMemberUpdatedComposer : IServerPacket
{
    private readonly int _groupId;
    private readonly Habbo _habbo;
    private readonly int _type;
    public int MessageId => ServerPacketHeader.GroupMemberUpdatedMessageComposer;

    public GroupMemberUpdatedComposer(int groupId, Habbo habbo, int type)
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