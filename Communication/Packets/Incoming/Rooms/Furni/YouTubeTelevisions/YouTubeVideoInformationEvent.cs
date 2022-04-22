using System.Linq;
using Plus.Communication.Packets.Outgoing.Rooms.Furni.YouTubeTelevisions;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Rooms.Furni.YouTubeTelevisions
{
    class YouTubeVideoInformationEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            var itemId = packet.PopInt();
            var videoId = packet.PopString();

            foreach (var tele in PlusEnvironment.GetGame().GetTelevisionManager().TelevisionList.ToList())
            {
                if (tele.YouTubeId != videoId)
                    continue;

                session.SendPacket(new GetYouTubeVideoComposer(itemId, tele.YouTubeId));
            }
        }
    }
}