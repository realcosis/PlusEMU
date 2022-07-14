using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users;

namespace Plus.Communication.Packets.Outgoing.Handshake;

public class UserObjectComposer : IServerPacket
{
    private readonly Habbo _habbo;
    public int MessageId => ServerPacketHeader.UserObjectMessageComposer;

    public UserObjectComposer(Habbo habbo)
    {
        _habbo = habbo;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_habbo.Id);
        packet.WriteString(_habbo.Username);
        packet.WriteString(_habbo.Look);
        packet.WriteString(_habbo.Gender.ToUpper());
        packet.WriteString(_habbo.Motto);
        packet.WriteString("");
        packet.WriteBoolean(false);
        packet.WriteInteger(_habbo.GetStats().Respect);
        packet.WriteInteger(_habbo.GetStats().DailyRespectPoints);
        packet.WriteInteger(_habbo.GetStats().DailyPetRespectPoints);
        packet.WriteBoolean(false); // Friends stream active
        packet.WriteString(_habbo.LastOnline.ToString()); // last online?
        packet.WriteBoolean(_habbo.ChangingName); // Can change name
        packet.WriteBoolean(false);
    }
}