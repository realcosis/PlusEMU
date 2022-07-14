using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Purse;

internal class CreditBalanceComposer : IServerPacket
{
    private readonly int _creditsBalance;
    public int MessageId => ServerPacketHeader.CreditBalanceMessageComposer;

    public CreditBalanceComposer(int creditsBalance)
    {
        _creditsBalance = creditsBalance;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteString(_creditsBalance + ".0");
}