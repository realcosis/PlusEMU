using Plus.Communication.Packets.Outgoing.Talents;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Talents;

internal class GetTalentTrackEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        var type = packet.PopString();
        var levels = PlusEnvironment.GetGame().GetTalentTrackManager().GetLevels();
        session.SendPacket(new TalentTrackComposer(levels, type));
    }
}