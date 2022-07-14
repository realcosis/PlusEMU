using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

internal class AvatarAspectUpdateMessageComposer : IServerPacket
{
    private readonly string _figure;
    private readonly string _gender;
    public int MessageId => ServerPacketHeader.AvatarAspectUpdateMessageComposer;

    public AvatarAspectUpdateMessageComposer(string figure, string gender)
    {
        _figure = figure;
        _gender = gender;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteString(_figure);
        packet.WriteString(_gender);
    }
}