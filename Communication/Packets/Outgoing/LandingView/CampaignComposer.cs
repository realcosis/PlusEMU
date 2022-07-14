using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.LandingView;

internal class CampaignComposer : IServerPacket
{
    private readonly string _campaignString;
    private readonly string _campaignName;

    public int MessageId => ServerPacketHeader.CampaignMessageComposer;

    public CampaignComposer(string campaignString, string campaignName)
    {
        _campaignString = campaignString;
        _campaignName = campaignName;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteString(_campaignString);
        packet.WriteString(_campaignName);
    }
}