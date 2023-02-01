using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users;

namespace Plus.Communication.Packets.Outgoing.Users;

public class GetRelationshipsComposer : IServerPacket
{
    private readonly Habbo _habbo;
    public uint MessageId => ServerPacketHeader.GetRelationshipsComposer;

    public GetRelationshipsComposer(Habbo habbo)
    {
        _habbo = habbo;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_habbo.Id);
        var relationships = _habbo.Messenger.Friends.Values.Where(f => f.Relationship > 0).GroupBy(f => f.Relationship).ToDictionary(f => f.Key, f => (f.First(), f.Count()));
        packet.WriteInteger(relationships.Count); // Count
        foreach (var (type, (friend, count)) in relationships)
        {
            packet.WriteInteger(type);
            packet.WriteInteger(count);
            packet.WriteInteger(friend.Id); // Their ID
            packet.WriteString(friend.Username);
            packet.WriteString(friend.Look);
        }

    }
}