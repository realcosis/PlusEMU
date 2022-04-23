using Plus.Communication.Attributes;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Handshake;

[NoAuth]
public class SsoTicketEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session == null || session.Rc4Client == null || session.GetHabbo() != null)
            return;
        var sso = packet.PopString();
        if (string.IsNullOrEmpty(sso) || sso.Length < 15)
            return;
        session.TryAuthenticate(sso);
    }
}