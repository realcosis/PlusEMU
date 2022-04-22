using System.Collections.Generic;

using Plus.HabboHotel.Talents;
using Plus.Communication.Packets.Outgoing.Talents;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Talents
{
    class GetTalentTrackEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            var type = packet.PopString();

            var levels = PlusEnvironment.GetGame().GetTalentTrackManager().GetLevels();

            session.SendPacket(new TalentTrackComposer(levels, type));
        }
    }
}
