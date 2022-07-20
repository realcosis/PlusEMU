using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Catalog;

public class CheckGnomeNameComposer : IServerPacket
{
    private readonly string _petName;
    private readonly int _errorId;
    public uint MessageId => ServerPacketHeader.CheckGnomeNameComposer;

    public CheckGnomeNameComposer(string petName, int errorId)
    {
        _petName = petName;
        _errorId = errorId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(0);
        packet.WriteInteger(_errorId);
        packet.WriteString(_petName);
    }
}