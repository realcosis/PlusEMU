using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Messenger;

internal class FriendListUpdateEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet) => Task.CompletedTask;
}