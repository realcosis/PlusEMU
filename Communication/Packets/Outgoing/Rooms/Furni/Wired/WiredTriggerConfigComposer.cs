using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items.Wired;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.Wired;

public class WiredTriggeRconfigComposer : IServerPacket
{
    private readonly IWiredItem _box;
    private readonly List<int> _blockedItems;

    public uint MessageId => ServerPacketHeader.WiredTriggeRconfigComposer;

    public WiredTriggeRconfigComposer(IWiredItem box, List<int> blockedItems)
    {
        _box = box;
        _blockedItems = blockedItems;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteBoolean(false);
        packet.WriteInteger(5);
        packet.WriteInteger(_box.SetItems.Count);
        foreach (var item in _box.SetItems.Values.ToList()) packet.WriteInteger(item.Id);
        packet.WriteInteger(_box.Item.GetBaseItem().SpriteId);
        packet.WriteInteger(_box.Item.Id);
        packet.WriteString(_box.StringData);
        packet.WriteInteger(_box is IWiredCycle ? 1 : 0);
        if (_box is IWiredCycle cycle) packet.WriteInteger(cycle.Delay);
        packet.WriteInteger(0);
        packet.WriteInteger(WiredBoxTypeUtility.GetWiredId(_box.Type));
        packet.WriteInteger(_blockedItems.Count);
        if (_blockedItems.Count > 0)
            foreach (var itemId in _blockedItems.ToList())
                packet.WriteInteger(itemId);

    }
}