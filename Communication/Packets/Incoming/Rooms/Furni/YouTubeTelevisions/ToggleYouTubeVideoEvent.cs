using Plus.Communication.Packets.Outgoing.Rooms.Furni.YouTubeTelevisions;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Rooms.Furni.YouTubeTelevisions
{
    class ToggleYouTubeVideoEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            var itemId = packet.PopInt();//Item Id
            var videoId = packet.PopString(); //Video ID

            session.SendPacket(new GetYouTubeVideoComposer(itemId, videoId));
        }
    }
}