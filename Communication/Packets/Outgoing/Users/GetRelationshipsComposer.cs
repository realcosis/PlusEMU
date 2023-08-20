using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Outgoing.Users;

public class GetRelationshipsComposer : IServerPacket
{
    private readonly int _userId;
    private readonly Dictionary<int, (MessengerBuddy buddy, int count)> _relationships;
    public uint MessageId => ServerPacketHeader.GetRelationshipsComposer;

    public GetRelationshipsComposer(int userId, Dictionary<int, (MessengerBuddy buddy, int count)> relationships)
    {
        _userId = userId;
        _relationships = relationships;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_userId);
        packet.WriteInteger(_relationships.Count); // Count

        foreach (var (type, (buddy, count)) in _relationships)
        {
            packet.WriteInteger(type);
            packet.WriteInteger(count);
            packet.WriteInteger(buddy.Id); // Their ID
            packet.WriteString(buddy.Username);
            packet.WriteString(buddy.Look);
        }
    }
}
