using Plus.Communication.Attributes;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Handshake;

[NoAuth]
internal class PingEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        session.PingCount = 0;
    }
}