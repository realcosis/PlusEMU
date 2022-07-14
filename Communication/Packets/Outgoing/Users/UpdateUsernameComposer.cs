using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Users;

internal class UpdateUsernameComposer : IServerPacket
{
    private readonly string _username;

    public int MessageId => ServerPacketHeader.UpdateUsernameMessageComposer;

    public UpdateUsernameComposer(string username)
    {
        _username = username;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(0);
        packet.WriteString(_username);
        packet.WriteInteger(0);
    }
}