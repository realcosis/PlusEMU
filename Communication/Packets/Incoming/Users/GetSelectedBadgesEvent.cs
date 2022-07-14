using Plus.Communication.Packets.Outgoing.Users;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Users;

internal class GetSelectedBadgesEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        var userId = packet.ReadInt();
        var habbo = PlusEnvironment.GetHabboById(userId);
        if (habbo == null)
            return Task.CompletedTask;
        session.Send(new HabboUserBadgesComposer(habbo));
        return Task.CompletedTask;
    }
}