using Plus.Communication.Packets.Outgoing.Users;
using Plus.HabboHotel.Friends;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Incoming.Users;

internal class GetRelationshipsEvent : IPacketEvent
{
    private readonly MessengerDataLoader _messengerDataLoader;
    private readonly GameClientManager _gameClientManager;

    public GetRelationshipsEvent(MessengerDataLoader messengerDataLoader, GameClientManager gameClientManager)
    {
        _messengerDataLoader = messengerDataLoader;
        _gameClientManager = gameClientManager;
    }

    public async Task Parse(GameClient session, IIncomingPacket packet)
    {
        var userId = packet.ReadInt();
        var client = _gameClientManager.GetClientByUserId(userId);
        Dictionary<int, (MessengerBuddy buddy, int count)> relationships;

        if (client != null)
        {
            // Online user logic
            relationships = client.GetHabbo().Messenger.GetRelationships(); // Replace with actual method
        }
        else
        {
            // Offline user logic
            relationships = await _messengerDataLoader.GetRelationshipsForUserAsync(userId);
        }

        session.Send(new GetRelationshipsComposer(userId, relationships));
    }
}
