using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items.Televisions;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.YouTubeTelevisions;

public class GetYouTubePlaylistComposer : IServerPacket
{
    private readonly int _itemId;
    private readonly ICollection<TelevisionItem> _videos;

    public uint MessageId => ServerPacketHeader.GetYouTubePlaylistComposer;

    public GetYouTubePlaylistComposer(int itemId, ICollection<TelevisionItem> videos)
    {
        _itemId = itemId;
        _videos = videos;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_itemId);
        packet.WriteInteger(_videos.Count);
        foreach (var video in _videos.ToList())
        {
            packet.WriteString(video.YouTubeId);
            packet.WriteString(video.Title); //Title
            packet.WriteString(video.Description); //Description
        }
        packet.WriteString("");

    }
}