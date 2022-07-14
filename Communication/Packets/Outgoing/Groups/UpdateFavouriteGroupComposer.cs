using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;

namespace Plus.Communication.Packets.Outgoing.Groups;

internal class UpdateFavouriteGroupComposer : IServerPacket
{
    private readonly Group? _group;
    private readonly int _virtualId;

    public int MessageId => ServerPacketHeader.UpdateFavouriteGroupMessageComposer;

    public UpdateFavouriteGroupComposer(Group? group, int virtualId)
    {
        _group = group;
        _virtualId = virtualId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_virtualId); //Sends 0 on .COM
        packet.WriteInteger(_group?.Id ?? 0);
        packet.WriteInteger(3);
        packet.WriteString(_group?.Name ?? string.Empty);
    }
}