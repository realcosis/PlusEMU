using Plus.Communication.Packets.Outgoing.Rooms.Furni.YouTubeTelevisions;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items.Televisions;

namespace Plus.Communication.Packets.Incoming.Rooms.Furni.YouTubeTelevisions;

internal class YouTubeVideoInformationEvent : IPacketEvent
{
    private readonly ITelevisionManager _televisionManager;

    public YouTubeVideoInformationEvent(ITelevisionManager televisionManager)
    {
        _televisionManager = televisionManager;
    }

    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        var itemId = packet.ReadInt();
        var videoId = packet.ReadString();
        foreach (var tele in _televisionManager.TelevisionList.ToList())
        {
            if (tele.YouTubeId != videoId)
                continue;
            session.Send(new GetYouTubeVideoComposer(itemId, tele.YouTubeId));
        }
        return Task.CompletedTask;
    }
}