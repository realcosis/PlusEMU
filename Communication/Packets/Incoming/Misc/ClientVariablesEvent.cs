using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Misc;

internal class ClientVariablesEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        var gordanPath = packet.ReadString();
        var externalVariables = packet.ReadString();
        return Task.CompletedTask;
    }
}