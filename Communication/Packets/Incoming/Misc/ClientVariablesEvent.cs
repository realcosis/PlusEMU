using Plus.Communication.Attributes;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Misc;

[NoAuthenticationRequired]
internal class ClientVariablesEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        var clientId = packet.ReadInt();
        var gordanPath = packet.ReadString();
        var externalVariables = packet.ReadString();
        return Task.CompletedTask;
    }
}