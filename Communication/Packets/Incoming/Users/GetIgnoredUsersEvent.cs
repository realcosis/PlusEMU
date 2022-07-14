using Plus.Communication.Packets.Outgoing.Users;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Ignores;

namespace Plus.Communication.Packets.Incoming.Users;

internal class GetIgnoredUsersEvent : IPacketEvent
{
    private readonly IIgnoredUsersService _ignoredUsersService;

    public GetIgnoredUsersEvent(IIgnoredUsersService ignoredUsersService)
    {
        _ignoredUsersService = ignoredUsersService;
    }

    public async Task Parse(GameClient session, IIncomingPacket packet)
    {
        var ignoredUsers = await _ignoredUsersService.GetIgnoredUsersByName(session.GetHabbo().IgnoresComponent.IgnoredUsers);
        session.Send(new IgnoredUsersComposer(ignoredUsers));
    }
}