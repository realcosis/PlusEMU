using Plus.Communication.Packets.Outgoing.Users;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Badges;
using Plus.HabboHotel.Users.UserData;

namespace Plus.Communication.Packets.Incoming.Users;

internal class GetSelectedBadgesEvent : IPacketEvent
{
    private readonly UserDataFactory _userDataFactory;
    private readonly GameClientManager _gameClientManager;

    public GetSelectedBadgesEvent(UserDataFactory userDataFactory, GameClientManager gameClientManager)
    {
        _userDataFactory = userDataFactory;
        _gameClientManager = gameClientManager;
    }

    public async Task Parse(GameClient session, IIncomingPacket packet)
    {
        var userId = packet.ReadInt();
        var gameClient = _gameClientManager.GetClientByUserId(userId);
        var habbo = gameClient?.GetHabbo();
        List<Badge> equippedBadges;

        if (habbo != null && habbo.Inventory?.Badges != null)
        {
            equippedBadges = habbo.Inventory.Badges.EquippedBadges;
        }
        else
        {
            equippedBadges = await _userDataFactory.GetEquippedBadgesForUserAsync(userId);
        }

        session.Send(new HabboUserBadgesComposer(userId, equippedBadges));
    }
}
