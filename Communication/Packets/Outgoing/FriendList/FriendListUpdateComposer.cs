using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Outgoing.FriendList;

public class FriendListUpdateComposer : IServerPacket
{
    private readonly Dictionary<MessengerBuddy, BuddyModificationType> _friends;
    public uint MessageId => ServerPacketHeader.FriendListUpdateComposer;

    public FriendListUpdateComposer(MessengerBuddy friend, BuddyModificationType modificationType)
    {
        _friends = new() { { friend, modificationType } };
    }

    public FriendListUpdateComposer(Dictionary<MessengerBuddy, BuddyModificationType> friends)
    {
        _friends = friends;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(0);
        packet.WriteInteger(_friends.Count);
        foreach (var (friend, modificationType) in _friends)
        {
            packet.WriteInteger((int)modificationType);
            if (modificationType == BuddyModificationType.Added || modificationType == BuddyModificationType.Updated)
                friend.Serialize(packet);
            else
                packet.WriteInteger(friend.Id);
        }
    }
}