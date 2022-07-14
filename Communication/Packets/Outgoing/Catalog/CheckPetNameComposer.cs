using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Catalog;

public class CheckPetNameComposer : IServerPacket
{
    private readonly int _error;
    private readonly string _extraData;
    public int MessageId => ServerPacketHeader.CheckPetNameMessageComposer;

    public CheckPetNameComposer(int error, string extraData)
    {
        _error = error;
        _extraData = extraData;
    }

    public void Compose(IOutgoingPacket packet)
    {
        //TODO: Create error enum
        packet.WriteInteger(_error); //0 = nothing, 1 = too long, 2 = too short, 3 = invalid characters
        packet.WriteString(_extraData);
    }
}