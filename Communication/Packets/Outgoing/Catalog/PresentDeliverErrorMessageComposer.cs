using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Catalog;

internal class PresentDeliverErrorMessageComposer : IServerPacket
{
    private readonly bool _creditError;
    private readonly bool _ducketError;

    public int MessageId => ServerPacketHeader.PresentDeliverErrorMessageComposer;

    public PresentDeliverErrorMessageComposer(bool creditError, bool ducketError)
    {
        _creditError = creditError;
        _ducketError = ducketError;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteBoolean(_creditError);
        packet.WriteBoolean(_ducketError);
    }
}