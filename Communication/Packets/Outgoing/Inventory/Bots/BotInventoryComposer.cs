using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Inventory.Bots;

namespace Plus.Communication.Packets.Outgoing.Inventory.Bots;

internal class BotInventoryComposer : IServerPacket
{
    private readonly ICollection<Bot> _bots;
    public int MessageId => ServerPacketHeader.BotInventoryMessageComposer;

    public BotInventoryComposer(ICollection<Bot> bots)
    {
        _bots = bots;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_bots.Count);
        foreach (var bot in _bots.ToList())
        {
            packet.WriteInteger(bot.Id);
            packet.WriteString(bot.Name);
            packet.WriteString(bot.Motto);
            packet.WriteString(bot.Gender);
            packet.WriteString(bot.Figure);
        }
    }
}