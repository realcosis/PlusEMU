using Plus.Communication.Packets.Outgoing.Users;
using Plus.HabboHotel.Friends;
using Plus.HabboHotel.GameClients;

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
        var relationships = await session.GetHabbo().Messenger.GetRelationshipsForUserAsync(userId, _gameClientManager, _messengerDataLoader);
        session.Send(new GetRelationshipsComposer(userId, relationships));
    }
}
